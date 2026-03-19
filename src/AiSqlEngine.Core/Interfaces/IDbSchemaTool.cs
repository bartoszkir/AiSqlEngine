using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface IDbSchemaTool
{
    Task<Result<string>> GetSchemaAsync(CancellationToken cancellationToken);
}