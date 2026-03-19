using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface ISqlExecutor
{
    Task<Result<string>> ExecuteAsync(string sql, CancellationToken cancellationToken);
}