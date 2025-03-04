using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltTextImageGenerator.TempOnnx;

internal class OnnxChatClient : IChatClient
{
    private string localOnnxModelPath;

    public OnnxChatClient(string localOnnxModelPath)
    {
        this.localOnnxModelPath = localOnnxModelPath;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
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
                if (content.Uri is not null && content.Data is null)
                {
                    imageLocation = content.Uri.ToString();
                }
                break;
            }
        }

        var img = Images.Load([imageLocation]);

        var fullPrompt = $"<|system|>{systemPrompt}<|end|><|user|><|image_1|>{userPrompt}<|end|><|assistant|>";

        // initialize model
        var model = new Model(localOnnxModelPath);
        var tokenizer = new Tokenizer(model);

        using MultiModalProcessor processor = new MultiModalProcessor(model);
        using var tokenizerStream = processor.CreateStream();

        // create the input tensor with the prompt and image
        var inputTensors = processor.ProcessImages(fullPrompt, img);
        using GeneratorParams generatorParams = new GeneratorParams(model);
        generatorParams.SetSearchOption("max_length", 3072);
        generatorParams.SetInputs(inputTensors);

        // generate response
        StringBuilder sb = new StringBuilder();
        using var generator = new Generator(model, generatorParams);
        while (!generator.IsDone())
        {
            //generator.ComputeLogits();
            generator.GenerateNextToken();
            var seq = generator.GetSequence(0)[^1];
            sb.Append(tokenizerStream.Decode(seq));
        }

        tokenizerStream.Dispose();
        processor.Dispose();
        tokenizer.Dispose();
        model.Dispose();

        var response = new ChatResponse(
            new ChatMessage(ChatRole.Assistant, sb.ToString()));

        return await Task.FromResult(response);
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
