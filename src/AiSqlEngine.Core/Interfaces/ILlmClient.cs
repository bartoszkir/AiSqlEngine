using AiSqlEngine.Core.Models;
using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface ILlmClient
{
    Task<Result<QueryPlan>> GenerateAsync(PromptMessage promptMessage, CancellationToken cancellationToken);
}