using Clowd.Clipboard;
using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using OpenAI;
using System;
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
        var clipboardImg = await ClipboardGdi.GetImageAsync();
        if (clipboardImg is null) return string.Empty;

        Console.WriteLine($"Found an image in clipboard");
        Console.WriteLine($"Clipboard image: {clipboardImg.Width}x{clipboardImg.Height}");

        string tempFile = $"{Guid.NewGuid()}.png";
        string currentAppLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string imagesFolder = Path.Combine(currentAppLocation, "images");

        Directory.CreateDirectory(imagesFolder);

        string tempFileFullPath = Path.Combine(imagesFolder, tempFile);
        clipboardImg.Save(tempFileFullPath);

        Console.WriteLine($"Saved image to: {tempFileFullPath}");
        Console.WriteLine();

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


        altText = $@"Generated alt text: 
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
                new ChatMessage(Microsoft.Extensions.AI.ChatRole.User, @$"Generate a complete alt text description for the attached image. Be very descriptive with the image description. Include all the elements and content found in the image. Return only the alt text description, no other content.")
            };

        StringBuilder stringBuilder = new StringBuilder();
        if (_settings.UseLocalOnnxModel)
        {
            Console.WriteLine($"Using Phi-4 local: {_settings.LocalOnnxModelPath}");
            var chatOnnxLocal = new OnnxPhi4ChatClient(_settings.LocalOnnxModelPath);

            // when use local onnx model, we only pass the image location
            messages.Add(new ChatMessage(Microsoft.Extensions.AI.ChatRole.User, [new DataContent(imageLocation, mediaType)]));

            var imageAnalysis = await chatOnnxLocal.GetResponseAsync(messages);
            stringBuilder.AppendLine("Phi-4: ");
            stringBuilder.AppendLine(imageAnalysis.Text);
            stringBuilder.AppendLine();
            Console.WriteLine($">> Phi-4 done");
            Console.WriteLine();
        }
        if (_settings.UseOllama)
        {
            Console.WriteLine($"Using Ollama: {_settings.OllamaModelId} - {_settings.OllamaUrl}");

            var uri = new Uri(_settings.OllamaUrl);
            var ollama = new OllamaApiClient(uri);
            ollama.SelectedModel = _settings.OllamaModelId;
            var chat = new Chat(ollama);

            var message = "Generate a complete alt text description for the attached image. Be very descriptive with the image description. Include all the elements and content found in the image. Return only the alt text description, no other content.";
            string imgDescription = "";
            // Convert the byte array to the required type: IEnumerable<IEnumerable<byte>>
            var imageBytesEnumerable = new List<IEnumerable<byte>> { imageBytes };

            await foreach (var answerToken in chat.SendAsync(message: message, imagesAsBytes: imageBytesEnumerable))            
                stringBuilder.Append(answerToken);

            Console.WriteLine($">> Ollama done");
            Console.WriteLine();
        }
        if (_settings.UseOpenAI)
        {
            Console.WriteLine($"Using OpenAI, ApiKey length: {_settings.OpenAIKey.Length}");
            var client = new OpenAI.OpenAIClient(apiKey: _settings.OpenAIKey);
            var chatOpenAI = client.GetChatClient(_settings.OpenAIModelId).AsIChatClient();

            // when using OpenAI the image should be added as byte array
            messages.Add(new ChatMessage(Microsoft.Extensions.AI.ChatRole.User, [new DataContent(imageBytes, mediaType)]));

            var imageAnalysis = await chatOpenAI.GetResponseAsync(messages);
            stringBuilder.AppendLine("OpenAI: ");
            stringBuilder.AppendLine(imageAnalysis.Text);
            stringBuilder.AppendLine();
            Console.WriteLine($">> OpenAI done");
            Console.WriteLine();
        }


        Console.WriteLine();
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