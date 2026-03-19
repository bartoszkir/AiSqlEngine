using AiSqlEngine.Core.Interfaces;
using AiSqlEngine.Core.Models;
using AiSqlEngine.Infrastructure.Configurations;
using Azure;
using Azure.AI.OpenAI;
using FluentResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI.Chat;

namespace AiSqlEngine.Infrastructure.Clients;

internal sealed class AzureOpenApiClient : ILlmClient
{
    private readonly OpenApiConfiguration _openApiConfiguration;

    public AzureOpenApiClient(IOptions<OpenApiConfiguration> openApiConfiguration)
    {
        _openApiConfiguration = openApiConfiguration.Value;
    }

    public async Task<Result<QueryPlan>> GenerateAsync(PromptMessage promptMessage, CancellationToken cancellationToken)
    {
        try
        {
            var azureClient = new AzureOpenAIClient(new Uri(_openApiConfiguration.Endpoint!),
                                                    new AzureKeyCredential(_openApiConfiguration.ApiKey!));

            var client = azureClient.GetChatClient(_openApiConfiguration.Deployment);

            var chatCompletionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 4096,
                Temperature = 0,
                TopP = 1.0f
            };

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(promptMessage.SystemMessage),
                new UserChatMessage(promptMessage.UserMessage)
            };

            var result = await client.CompleteChatAsync(messages, chatCompletionOptions, cancellationToken: cancellationToken);
            var queryPlan = JsonConvert.DeserializeObject<QueryPlan>(result.Value.Content[0].Text);
            return Result.Ok(queryPlan!);
        }
        catch (Exception e)
        {
            return Result.Fail<QueryPlan>($"AI does not work. Error: {e}");
        }
    }
}