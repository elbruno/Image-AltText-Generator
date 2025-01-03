using Microsoft.Extensions.AI;
using System;
using System.Diagnostics;
using TextCopy;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "-h")
        {
            ShowHelp();
            return;
        }

        string fileLocation = args[0];
        Console.WriteLine($"Processing file at location: {fileLocation}");

        // generate the alt text for the image
        string altText = await GenerateAltTextForImageAsync(fileLocation);

        // show the genarated alt text
        Console.WriteLine($"Generated alt text: {altText}");

        // copy the alt text content to the clipboard
        ClipboardService.SetText(altText);

        // create a new file with the alt text, with the name of the original file and the TXT extension
        string altTextFileLocation = Path.ChangeExtension(fileLocation, ".txt");
        File.WriteAllText(altTextFileLocation, altText);

        Console.WriteLine($"Alt text generated, copied to clipboard and saved to: {altTextFileLocation}");
    }

    static void ShowHelp()
    {
        Console.WriteLine("Usage: AltTextImageGenerator <fileLocation> [-h]");
        Console.WriteLine("Options:");
        Console.WriteLine("  -h    Show help information");
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
}
