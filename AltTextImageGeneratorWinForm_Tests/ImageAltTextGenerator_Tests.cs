using AltTextImageGenerator;
using Clowd.Clipboard;

namespace AltTextImageGeneratorWinForm_Tests
{
    [TestClass]
    public sealed class ImageAltTextGenerator_Tests
    {
        [TestMethod]
        public async Task GetImageFromClipboardAndSaveItToDisk_ImageInClipboard_ReturnsFilePath()
        {
            // Arrange
            var settings = RetrieveCurrentAppSettings();
            var generator = new ImageAltTextGenerator(settings);

            var imageTest = ResourcesTest.foggydaysmall;
            ClipboardGdi.SetImage(imageTest);

            // Act
            string result = await generator.GetImageFromClipboardAndSaveItToDisk();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(File.Exists(result));

            // Cleanup
            if (File.Exists(result))
            {
                File.Delete(result);
            }
        }

        [TestMethod]
        public async Task GetImageFromClipboardAndSaveItToDisk_NoImageInClipboard_ReturnsEmptyString()
        {
            // Arrange
            var settings = RetrieveCurrentAppSettings();
            var generator = new ImageAltTextGenerator(settings);
            ClipboardGdi.Empty();

            // Act
            string result = await generator.GetImageFromClipboardAndSaveItToDisk();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        static AltTextGenSettings RetrieveCurrentAppSettings()
        {
            AltTextGenSettings settings = new()
            {
                UseOllama = true,
                OllamaUrl = "http://localhost:11434/",
                OllamaModelId = "llama3.2-vision",
                UseOpenAI = false,
                OpenAIKey = "",
                OpenAIModelId = ""
            };
            return settings;
        }
    }
}

