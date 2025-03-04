namespace AltTextImageGenerator;

public class AltTextGenSettings
{
    public AltTextGenSettings()
    {
        UseOllama = true;
        OllamaUrl = "http://localhost:11434";
        OllamaModelId = "llama3.2-vision";
        UseOpenAI = false;
        OpenAIKey = string.Empty;
        OpenAIModelId = "gpt-4o-mini";
        UseLocalOnnxModel = false;
        LocalOnnxModelPath = string.Empty;

    }
    public bool UseOllama {get; set;}
    public string OllamaUrl {get; set;}
    public string OllamaModelId {get; set;}
    public bool UseOpenAI {get; set;}
    public string OpenAIKey {get; set;}
    public string OpenAIModelId { get; set; }
    public bool UseLocalOnnxModel { get; set; }
    public string LocalOnnxModelPath { get; set; }
}
