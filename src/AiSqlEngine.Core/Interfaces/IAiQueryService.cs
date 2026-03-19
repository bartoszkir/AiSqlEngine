using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface IAiQueryService
{
    Task<Result<string>> ConvertToSqlAsync(string message, CancellationToken cancellationToken);
    Task<Result<string>> ExecuteAsync(string  sql, CancellationToken cancellationToken);
}