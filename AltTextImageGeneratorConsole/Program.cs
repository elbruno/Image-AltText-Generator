using AltTextImageGenerator;
using AltTextImageGeneratorConsole.Settings;
using System.Text.Json;

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
            case "-help":
                ShowHelp();
                break;
            case "-show":
                ShowCurrentSettingsAsync();
                break;
            case "-set":
                if (args.Length < 3)
                {
                    Console.WriteLine("Please provide a key and a value to set.");
                    break;
                }
                SetSettingAsync(args[1], args[2]);
                Console.WriteLine($"Setting {args[1]} to {args[2]}");
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
-help Show help information
-show Show current settings
-set <key> <value> Save a setting value";
    Console.WriteLine(messageInfo);
}

static void SetSettingAsync(string key, string value)
{
    var settingsRepo = new FileSystemSettingsRepo();
    var userSettings = settingsRepo.GetSettings().Result;

    var propertyInfo = typeof(AltTextGenSettings).GetProperty(key);
    if (propertyInfo == null)
    {
        Console.WriteLine($"Setting {key} not found.");
        return;
    }
    if (propertyInfo.PropertyType == typeof(bool))
    {
        propertyInfo.SetValue(userSettings, bool.Parse(value));
    }
    else if (propertyInfo.PropertyType == typeof(string))
    {
        propertyInfo.SetValue(userSettings, value);
    }
    else
    {
        Console.WriteLine($"Setting {key} is not a string or boolean.");
        return;
    }
    settingsRepo.SaveSettings(userSettings);
}

static AltTextGenSettings RetrieveCurrentAppSettings()
{
    var settingsRepo = new FileSystemSettingsRepo();
    var userSettings = settingsRepo.GetSettings().Result;

    return new AltTextGenSettings
    {
        UseOllama = userSettings.UseOllama,
        OllamaUrl = userSettings.OllamaUrl,
        OllamaModelId = userSettings.OllamaModelId,
        UseOpenAI = userSettings.UseOpenAI,
        OpenAIKey = userSettings.OpenAIKey,
        OpenAIModelId = userSettings.OpenAIModelId,
        UseLocalOnnxModel = userSettings.UseLocalOnnxModel,
        LocalOnnxModelPath = userSettings.LocalOnnxModelPath
    };
}

static void ShowCurrentSettingsAsync()
{
    var settingsRepo = new FileSystemSettingsRepo();
    var userSettings = settingsRepo.GetSettings().Result;

    var settingsJson = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(settingsJson);
    Console.WriteLine();
    Console.WriteLine("Settings are stored in the userSettings.json file in the LocalAppData folder.");
}
