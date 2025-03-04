using AltTextImageGenerator;
using System.Text.Json;

namespace AltTextImageGeneratorConsole.Settings;

sealed class FileSystemSettingsRepo : ISettingsRepo
{
    // Can pass those as constructor params or whatever.
    private const string AppName = "Settings";
    private const string SettingsFileName = "userSettings.json";

    private readonly string _settingsFolderPath = GetSettingsFolderPath();
    private readonly string _settingsFilePath = GetSettingsFilePath();

    public async Task<AltTextGenSettings> GetSettings()
    {
        if (!File.Exists(_settingsFilePath))
        {
            await SaveSettings(new AltTextGenSettings());
        }

        using var settingsFileStream = new FileStream(_settingsFilePath, FileMode.Open);
        var deserializedSettings = await JsonSerializer.DeserializeAsync<AltTextGenSettings>(settingsFileStream);

        if (deserializedSettings is null)
        {
            throw new InvalidOperationException("Can't deserialize user settings");
        }

        return deserializedSettings;
    }

    public async Task SaveSettings(AltTextGenSettings userSettings)
    {
        if (!Directory.Exists(_settingsFolderPath))
        {
            Directory.CreateDirectory(_settingsFolderPath);
        }

        using var settingsFileStream = new FileStream(_settingsFilePath, FileMode.OpenOrCreate);
        await JsonSerializer.SerializeAsync(settingsFileStream, userSettings);
    }

    private static string GetSettingsFolderPath()
    {
        var currentAppFolder = AppContext.BaseDirectory;
        return Path.Combine(currentAppFolder, AppName);
    }

    private static string GetSettingsFilePath()
    {
        var settingsFolder = GetSettingsFolderPath();
        return Path.Combine(settingsFolder, SettingsFileName);
    }
}