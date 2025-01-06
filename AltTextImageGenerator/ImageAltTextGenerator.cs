using Clowd.Clipboard;
using Microsoft.Extensions.AI;
using OpenAI;
using System.Reflection;
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

        var aiServiceUsed = _settings.UseOllama ? "Ollama" : "OpenAI";

        altText = $@"Using AI service: {aiServiceUsed}
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
        IChatClient chat;
        if (_settings.UseOllama)
        {
            chat = new OllamaChatClient(
                new Uri(uriString: _settings.OllamaUrl),
                _settings.OllamaModelId);
        }
        else if (_settings.UseOpenAI)
        {
            chat = new OpenAIClient(apiKey: _settings.OpenAIKey).AsChatClient(_settings.OpenAIModelId);
        }
        else
        {
            throw new NotSupportedException("No AI service is configured");
        }

        // get the media type from the image location
        var mediaType = GetMediaType(imageLocation);

        List<ChatMessage> messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.User, @$"Generate the alt text description for the attached image. Return only the alt text description, no other content."),
                new ChatMessage(ChatRole.User, [new ImageContent(File.ReadAllBytes(imageLocation), mediaType)])
            };
        // send the messages to the assistant            
        var imageAnalysis = await chat.CompleteAsync(messages);
        return imageAnalysis.Message.Text;
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