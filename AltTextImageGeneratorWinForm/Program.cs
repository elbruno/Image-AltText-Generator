using AltTextImageGenerator;

namespace AltTextImageGeneratorWinForm;
internal static class Program
{
    [STAThread]
    static async Task Main(string[] args)
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var imageAltTextGenerator = new ImageAltTextGenerator(RetrieveCurrentAppSettings());

        try
        {
            if (args.Length == 0)
            {
                var message = await imageAltTextGenerator.ValidateIfClipboardIsImageAsync();
                Console.WriteLine(message);
                MessageBox.Show(message);
            }
            else
            {
                switch (args[0])
                {
                    case "-h":
                        ShowHelp();
                        break;
                    case "-r":
                        if (RegisterAppInWindowsContextMenu.AddContextMenu())
                            MessageBox.Show("Application registered in Windows menus.");
                        break;
                    case "-s":
                        using (var settingsForm = new formSettings())
                        {
                            settingsForm.ShowDialog();
                        }
                        break;
                    default:
                        var message = await imageAltTextGenerator.ProcessImageFile(args[0]);
                        Console.WriteLine(message);
                        MessageBox.Show(message);
                        break;
                }
            }
        }
        catch (Exception exc)
        {
            MessageBox.Show($"Error: {exc.Message}");
        }
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
-r Register the app in Windows, to be show in the right click menu on images
-s Open the settings for the Application";
        MessageBox.Show(messageInfo, "Help Information");
    }

    static AltTextGenSettings RetrieveCurrentAppSettings()
    {
        Properties.Settings.Default.Reload();
        return new AltTextGenSettings
        {
            UseOllama = Properties.Settings.Default.UseOllama,
            OllamaUrl = Properties.Settings.Default.OllamaUrl,
            OllamaModelId = Properties.Settings.Default.OllamaModelId,
            UseOpenAI = Properties.Settings.Default.UseOpenAI,
            OpenAIKey = Properties.Settings.Default.OpenAIKey,
            OpenAIModelId = Properties.Settings.Default.OpenAIModel,
            UseLocalOnnxModel = Properties.Settings.Default.UseLocalOnnxModel,
            LocalOnnxModelPath = Properties.Settings.Default.LocalOnnxModelPath
        };
    }
}
