using AltTextImageGenerator;

namespace AltTextImageGeneratorConsole.Settings;

interface ISettingsRepo
{
    Task<AltTextGenSettings> GetSettings();

    Task SaveSettings(AltTextGenSettings altTextGenSettings);
}
