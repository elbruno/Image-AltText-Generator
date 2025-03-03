namespace AltTextImageGenerator;

public class AltTextGenSettings
{
    public bool UseOllama {get; set;}
    public string OllamaUrl {get; set;}
    public string OllamaModelId {get; set;}
    public bool UseOpenAI {get; set;}
    public string OpenAIKey {get; set;}
    public string OpenAIModelId { get; set; }
    public bool UseLocalOnnxModel { get; set; }
    public string LocalOnnxModelPath { get; set; }
}
