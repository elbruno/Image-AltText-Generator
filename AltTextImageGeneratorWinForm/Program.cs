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

            if (args.Length == 0)
            {
                await ImageAltTextGenerator.ValidateIfClipboardIsImageAsync();
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
                await ImageAltTextGenerator.ProcessImageFile(fileLocation);
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
    }
}
