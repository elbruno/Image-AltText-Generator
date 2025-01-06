using AltTextImageGenerator;
using System.Linq.Expressions;

namespace AltTextImageGeneratorWinForm
{
    internal static class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var imageAltTextGenerator = new ImageAltTextGenerator(RetrieveCurrentAppSettings());

            if (args.Length == 0)
            {
                try
                {
                    var message = await imageAltTextGenerator.ValidateIfClipboardIsImageAsync();
                    MessageBox.Show(message);
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Error: {exc.Message}");
                }
                return;
            }
            if (args[0] == "-h")
            {
                ShowHelp();
                return;
            }

            if (args[0] == "-r")
            {
                var success = RegisterAppInWindowsContextMenu.RegisterAppInWindowsMenus();
                if (success)
                    MessageBox.Show("Application registered in Windows menus.");
                return;
            }

            if (args[0] == "-s")
            {
                formSettings settingsForm = new();
                settingsForm.ShowDialog();
                return;
            }

            try
            {
                string fileLocation = args[0];
                await imageAltTextGenerator.ProcessImageFile(fileLocation);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
        }


        static void ShowHelp()
        {
            var messageInfo = @"Usage: 

AltTextImageGenerator < no arguments >
  Validate if the clipboard contains an image and process it

AltTextImageGenerator <fileLocation>
  Where <fileLocation> the image to be processed and the Alt-Text generated

Options:  
-h Show help information
-r Register the app in Windows, to be show in the right click menu on images
-s Open the settings for the Application";
            MessageBox.Show(messageInfo, "Help Information");
        }

        static AltTextGenSettings RetrieveCurrentAppSettings()
        {
            Properties.Settings.Default.Reload();
            AltTextGenSettings settings = new()
            {
                UseOllama = Properties.Settings.Default.UseOllama,
                OllamaUrl = Properties.Settings.Default.OllamaUrl,
                OllamaModelId = Properties.Settings.Default.OllamaModelId,
                UseOpenAI = Properties.Settings.Default.UseOpenAI,
                OpenAIKey = Properties.Settings.Default.OpenAIKey,
                OpenAIModelId = Properties.Settings.Default.OpenAIModel
            };
            return settings;
        }

    }
}
