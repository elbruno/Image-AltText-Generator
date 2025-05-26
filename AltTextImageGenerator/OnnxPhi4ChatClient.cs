using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using System.Text;

namespace AltTextImageGenerator;

internal class OnnxPhi4ChatClient : IChatClient, IDisposable
{
    private readonly string localOnnxModelPath;
    private readonly Model model;
    private readonly Tokenizer tokenizer;

    public OnnxPhi4ChatClient(string localOnnxModelPath)
    {
        this.localOnnxModelPath = localOnnxModelPath;
        model = new Model(localOnnxModelPath);
        tokenizer = new Tokenizer(model);
    }

    public void Dispose()
    {
        tokenizer.Dispose();
        model.Dispose();
    }

    public async Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        var systemPrompt = "";
        var userPrompt = "";
        var imageLocation = "";

        foreach (var message in chatMessages)
        {
            foreach (var content in message.Contents.OfType<TextContent>())
            {
                userPrompt = content.Text;
                break;
            }

            foreach (var content in message.Contents.OfType<DataContent>())
            {
                if (content.Uri is not null)
                {
                    imageLocation = content.Uri.ToString();
                }
                break;
            }
        }

        var img = Images.Load([imageLocation]);

        var fullPrompt = $"<|system|>{systemPrompt}<|end|><|user|><|image_1|>{userPrompt}<|end|><|assistant|>";

        using MultiModalProcessor processor = new MultiModalProcessor(model);
        using var tokenizerStream = processor.CreateStream();

        var inputTensors = processor.ProcessImages(fullPrompt, img);
        using GeneratorParams generatorParams = new GeneratorParams(model);
        generatorParams.SetSearchOption("max_length", 3072);
        generatorParams.SetInputs(inputTensors);

        StringBuilder sb = new StringBuilder();
        using var generator = new Generator(model, generatorParams);
        while (!generator.IsDone())
        {
            generator.GenerateNextToken();
            var seq = generator.GetSequence(0)[^1];
            sb.Append(tokenizerStream.Decode(seq));
        }

        var response = new ChatResponse(
            new ChatMessage(ChatRole.Assistant, sb.ToString()));

        return await Task.FromResult(response);
    }

    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}