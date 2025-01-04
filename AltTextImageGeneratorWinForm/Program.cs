using Clowd.Clipboard;
using Microsoft.Extensions.AI;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Reflection;
using TextCopy;

namespace AltTextImageGeneratorWinForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (args.Length == 0)
            {
                await ValidateIfClipboardIsImageAsync();
                return;
            }
            if (args[0] == "-h")
            {
                ShowHelp();
                return;
            }

            if (args[0] == "-r")
            {
                var success = RegisterAppInWindowsMenus();
                if (success)
                    MessageBox.Show("Application registered in Windows menus.");
                return;
            }

            try
            {
                string fileLocation = args[0];
                await ProcessImageFile(fileLocation);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
        }

        private static async Task ProcessImageFile(string fileLocation)
        {
            try
            {
                // generate the alt text for the image
                string altText = await GenerateAltTextForImageAsync(fileLocation);

                // create a new file with the alt text, with the name of the original file and the TXT extension
                string altTextFileLocation = Path.ChangeExtension(fileLocation, ".txt");
                File.WriteAllText(altTextFileLocation, altText);

                MessageBox.Show($@"Source file: {fileLocation}
Alt Text file: {altTextFileLocation}

Generated alt text: 
{altText}");

                // copy the alt text content to the clipboard
                ClipboardService.SetText(altText);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }

        }

        private static async Task ValidateIfClipboardIsImageAsync()
        {
            var clipboardImg = await ClipboardGdi.GetImageAsync();

            if (clipboardImg is not null)
            {
                // generate a random file name with the extension png
                string tempFile = $"{Guid.NewGuid().ToString()}.png";

                // get the current app location
                string currentAppLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string imagesFolder = Path.Combine(currentAppLocation, "images");
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);
                string tempFileFullPath = Path.Combine(imagesFolder, tempFile);
                clipboardImg.Save(tempFileFullPath); //, ImageFormat.Png);

                await ProcessImageFile(tempFileFullPath);
            }
        }

        static void ShowHelp()
        {
            var messageInfo = @"Usage: AltTextImageGenerator <fileLocation> 
<fileLocation> the image to be processed and the Alt Text generated
Options:  
-h Show help information
-r Register the app in Windows, to be show in the right click menu on images";
            MessageBox.Show(messageInfo, "Help Information");
        }

        static async Task<string> GenerateAltTextForImageAsync(string imageLocation)
        {
            var chat = new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2-vision");

            // get the media type from the image location
            var mediaType = GetMediaType(imageLocation);

            List<ChatMessage> messages = [
                new ChatMessage(ChatRole.User, @$"Generate the alt text description for the attached image. Return only the alt text description, no other content."),
            new ChatMessage(ChatRole.User, [new ImageContent(File.ReadAllBytes(imageLocation), mediaType)])
                ];
            // send the messages to the assistant            
            var imageAnalysis = await chat.CompleteAsync(messages);
            return imageAnalysis.Message.Text;
        }

        static string GetMediaType(string imageLocation)
        {
            // Logic to determine the media type based on the file extension
            string extension = Path.GetExtension(imageLocation).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    throw new NotSupportedException($"File extension {extension} is not supported");
            }
        }

        static bool RegisterAppInWindowsMenus()
        {
            var result = false;
            try
            {
                // get the current app path
                string appPath = Assembly.GetExecutingAssembly().Location;

                // Define the application path (assumes the app is in the same folder as this script)
                string command = $"\"{appPath}\" \"%1\"";

                // File extensions to associate with the context menu
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

                foreach (var ext in imageExtensions)
                {
                    // Open or create the key for the file extension
                    string extKeyPath = $@"Software\Classes\{ext}\shell\Generate Alt Text";
                    using (RegistryKey extKey = Registry.CurrentUser.CreateSubKey(extKeyPath))
                    {
                        // Set the display name for the menu item
                        extKey.SetValue(null, "Generate Alt Text");
                    }

                    // Add the command to run the app
                    string commandKeyPath = $@"Software\Classes\{ext}\shell\Generate Alt Text\command";
                    using (RegistryKey commandKey = Registry.CurrentUser.CreateSubKey(commandKeyPath))
                    {
                        commandKey.SetValue(null, command);
                    }
                }

                result = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show($@"Error while registering the app in Windows Context Menu.

Error Details: {exc.Message}");
            }
            return result;
        }
    }
}
