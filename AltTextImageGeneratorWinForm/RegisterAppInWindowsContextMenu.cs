using Microsoft.Win32;
using System.Reflection;

namespace AltTextImageGeneratorWinForm;

internal class RegisterAppInWindowsContextMenu
{
    internal static bool RegisterAppInWindowsMenus()
    {
        var result = false;
        try
        {
            // get the current app path
            string appPath = Assembly.GetExecutingAssembly().Location;

            // Define the application path (assumes the app is in the same folder as this script)
            string command = $"\"{appPath}\" \"%1\"";

            // File extensions to associate with the context menu
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

            foreach (var ext in imageExtensions)
            {
                // Open or create the key for the file extension
                string extKeyPath = $@"Software\Classes\{ext}\shell\Generate Alt Text";
                using (RegistryKey extKey = Registry.CurrentUser.CreateSubKey(extKeyPath))
                {
                    // Set the display name for the menu item
                    extKey.SetValue(null, "Generate Alt Text");
                }

                // Add the command to run the app
                string commandKeyPath = $@"Software\Classes\{ext}\shell\Generate Alt Text\command";
                using (RegistryKey commandKey = Registry.CurrentUser.CreateSubKey(commandKeyPath))
                {
                    commandKey.SetValue(null, command);
                }
            }

            result = true;
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"Error while registering the app in Windows Context Menu.

Error Details: {exc.Message}");
        }
        return result;
    }
}
