using Clowd.Clipboard;
using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using OpenAI;
using System.Reflection;
using System.Text;
using TextCopy;

namespace AltTextImageGenerator;

public class ImageAltTextGenerator
{
    private readonly AltTextGenSettings _settings;

    public ImageAltTextGenerator(AltTextGenSettings settings) => _settings = settings;

    public async Task<string> ValidateIfClipboardIsImageAsync()
    {
        string fileFromClipboard = await GetImageFromClipboardAndSaveItToDisk(); 
        return await ProcessImageFile(fileFromClipboard);
    }

    public async Task<string> GetImageFromClipboardAndSaveItToDisk()
    {
        string tempFileFullPath = "";
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
            tempFileFullPath = Path.Combine(imagesFolder, tempFile);
            clipboardImg.Save(tempFileFullPath);
        }

        return tempFileFullPath;
    }

    public async Task<string> ProcessImageFile(string fileLocation)
    {
        // generate the alt text for the image
        var altText = await GenerateAltTextForImageAsync(fileLocation);

        // create a new file with the alt text, with the name of the original file and the TXT extension
        string altTextFileLocation = Path.ChangeExtension(fileLocation, ".txt");
        File.WriteAllText(altTextFileLocation, altText);

        // create a new string that indicates which services are used
        // and the location of the source file and the alt text file

        var aiServiceUsed = $@"Use OpenAI: {_settings.UseOpenAI}
Use Ollama: {_settings.UseOllama} - {_settings.OllamaModelId}
Use Phi-4: {_settings.UseLocalOnnxModel}";


        altText = $@"AI services:{Environment.NewLine}{aiServiceUsed}
Source file: {fileLocation}
Alt Text file: {altTextFileLocation}

Generated alt text: 
{altText}";

        // copy the alt text content to the clipboard
        ClipboardService.SetText(altText);

        return altText;
    }

    private async Task<string> GenerateAltTextForImageAsync(string imageLocation)
    {
        // get the media type from the image location
        var mediaType = GetMediaType(imageLocation);

        byte[] imageBytes = File.ReadAllBytes(imageLocation);


        List<ChatMessage> messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.User, @$"Generate the alt text description for the attached image. Return only the alt text description, no other content."),
                new ChatMessage(ChatRole.User, [new DataContent(imageLocation, mediaType)]),
                new ChatMessage(ChatRole.User, [new DataContent(imageBytes, mediaType)])
            };

        StringBuilder stringBuilder = new StringBuilder();

        if (_settings.UseOllama)
        {
            var chatOllama = new OllamaChatClient(
                new Uri(uriString: _settings.OllamaUrl),
                _settings.OllamaModelId);
            var imageAnalysis = await chatOllama.GetResponseAsync(messages);
            stringBuilder.AppendLine("Ollama: ");
            stringBuilder.AppendLine(imageAnalysis.Message.Text);
            stringBuilder.AppendLine();
        }
        if (_settings.UseOpenAI)
        {
            var chatOpenAI = new OpenAI.OpenAIClient(apiKey: _settings.OpenAIKey).AsChatClient(_settings.OpenAIModelId);
            var imageAnalysis = await chatOpenAI.GetResponseAsync(messages);
            stringBuilder.AppendLine("OpenAI: ");
            stringBuilder.AppendLine(imageAnalysis.Message.Text);
            stringBuilder.AppendLine();
        }
        if (_settings.UseLocalOnnxModel)
        {            
            var chatOnnxLocal = new OnnxPhi4ChatClient(_settings.LocalOnnxModelPath);
            var imageAnalysis = await chatOnnxLocal.GetResponseAsync(messages);
            stringBuilder.AppendLine("Phi-4: ");
            stringBuilder.AppendLine(imageAnalysis.Message.Text);
            stringBuilder.AppendLine();
        }
        else
        {
            throw new NotSupportedException("No AI service is configured");
        }

        return stringBuilder.ToString();
    }

    public string GetMediaType(string imageLocation)
    {
        // Logic to determine the media type based on the file extension
        string extension = Path.GetExtension(imageLocation).ToLower();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => throw new NotSupportedException($"File extension {extension} is not supported"),
        };
    }
}