using AltTextImageGenerator;
using System.Reflection;

namespace AltTextImageGenerator_Tests;

[TestClass]
public class GetMediaType_Tests
{
    private ImageAltTextGenerator _generator;

    [TestInitialize]
    public void Setup()
    {
        var settings = new AltTextGenSettings
        {
            UseOllama = true,
            OllamaUrl = "http://localhost:11434/",
            OllamaModelId = "llama3.2-vision",
            UseOpenAI = false,
            OpenAIKey = "",
            OpenAIModelId = ""
        };
        _generator = new ImageAltTextGenerator(settings);
    }

    [TestMethod]
    [DataRow(".jpg", "image/jpeg")]
    [DataRow(".jpeg", "image/jpeg")]
    [DataRow(".png", "image/png")]
    [DataRow(".gif", "image/gif")]
    public void GetMediaType_ValidExtensions_ReturnsCorrectMediaType(string extension, string expectedMediaType)
    {
        // Arrange
        string filePath = $"test{extension}";

        // Act
        string mediaType = _generator.GetMediaType(filePath);

        // Assert
        Assert.AreEqual(expectedMediaType, mediaType);
    }

    [TestMethod]
    [DataRow(".bmp")]
    [DataRow(".tiff")]
    [DataRow(".svg")]
    public void GetMediaType_InvalidExtensions_ThrowsNotSupportedException(string extension)
    {
        // Arrange
        string filePath = $"test{extension}";

        // Act & Assert
        Assert.ThrowsException<NotSupportedException>(() => _generator.GetMediaType(filePath));
    }
}
