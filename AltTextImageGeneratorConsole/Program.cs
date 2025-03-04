using AltTextImageGenerator;
using Microsoft.Extensions.Configuration;

var imageAltTextGenerator = new ImageAltTextGenerator(RetrieveCurrentAppSettings());

try
{
    if (args.Length == 0)
    {
        var message = await imageAltTextGenerator.ValidateIfClipboardIsImageAsync();
        Console.WriteLine(message);
    }
    else
    {
        switch (args[0])
        {
            case "-h":
                ShowHelp();
                break;
            case "-s":
                ShowCurrentSettings();
                break;
            default:
                var message = await imageAltTextGenerator.ProcessImageFile(args[0]);
                Console.WriteLine(message);
                break;
        }
    }
}
catch (Exception exc)
{
    Console.WriteLine($"Error: {exc.Message}");
}

static void ShowHelp()
{
    const string messageInfo = @"Usage: 

AltTextImageGenerator < no arguments >
  Validate if the clipboard contains an image and process it

AltTextImageGenerator <fileLocation>
  Where <fileLocation> the image to be processed and the Alt-Text generated

Options:  
-h Show help information
-s Show current settings";
    Console.WriteLine(messageInfo);
}

static AltTextGenSettings RetrieveCurrentAppSettings()
{
    var configuration = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

    return new AltTextGenSettings
    {
        UseOllama = bool.Parse(configuration["UseOllama"]),
        OllamaUrl = configuration["OllamaUrl"],
        OllamaModelId = configuration["OllamaModelId"],
        UseOpenAI = bool.Parse(configuration["UseOpenAI"]),
        OpenAIKey = configuration["OpenAIKey"],
        OpenAIModelId = configuration["OpenAIModelId"],
        UseLocalOnnxModel = bool.Parse(configuration["UseLocalOnnxModel"]),
        LocalOnnxModelPath = configuration["LocalOnnxModelPath"]
    };
}

static void ShowCurrentSettings()
{
    var settings = RetrieveCurrentAppSettings();
    Console.WriteLine($"UseOllama: {settings.UseOllama}");
    Console.WriteLine($"OllamaUrl: {settings.OllamaUrl}");
    Console.WriteLine($"OllamaModelId: {settings.OllamaModelId}");
    Console.WriteLine($"UseOpenAI: {settings.UseOpenAI}");
    Console.WriteLine($"OpenAIKey: {settings.OpenAIKey}");
    Console.WriteLine($"OpenAIModelId: {settings.OpenAIModelId}");
    Console.WriteLine($"UseLocalOnnxModel: {settings.UseLocalOnnxModel}");
    Console.WriteLine($"LocalOnnxModelPath: {settings.LocalOnnxModelPath}");
}
