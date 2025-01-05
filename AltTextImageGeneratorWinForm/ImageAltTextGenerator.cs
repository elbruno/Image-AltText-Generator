using Clowd.Clipboard;
using Microsoft.Extensions.AI;
using OpenAI;
using System.Reflection;
using TextCopy;

namespace AltTextImageGeneratorWinForm
{
    internal static class ImageAltTextGenerator
    {

        internal static async Task ValidateIfClipboardIsImageAsync()
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

        internal static async Task ProcessImageFile(string fileLocation)
        {
            try
            {
                // generate the alt text for the image
                string altText = await GenerateAltTextForImageAsync(fileLocation);

                // create a new file with the alt text, with the name of the original file and the TXT extension
                string altTextFileLocation = Path.ChangeExtension(fileLocation, ".txt");
                File.WriteAllText(altTextFileLocation, altText);

                var aiServiceUsed = Properties.Settings.Default.UseOllama ? "Ollama" : "OpenAI";  

                MessageBox.Show($@"Using AI service: {aiServiceUsed}
Source file: {fileLocation}
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

        static async Task<string> GenerateAltTextForImageAsync(string imageLocation)
        {
            IChatClient chat; 
            if(Properties.Settings.Default.UseOllama)
            {
                chat = new OllamaChatClient(
                    new Uri(Properties.Settings.Default.OllamaUrl),
                    Properties.Settings.Default.OllamaModelId);
            }
            else if (Properties.Settings.Default.UseOpenAI)
            {                
                chat = new OpenAIClient(apiKey: Properties.Settings.Default.OpenAIKey).AsChatClient(Properties.Settings.Default.OpenAIModel);
            }
            else
            {
                throw new NotSupportedException("No AI service is configured");
            }

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
    }
}
