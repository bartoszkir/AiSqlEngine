using AiSqlEngine.Core.Builders;
using AiSqlEngine.Core.Interfaces;
using FluentResults;

namespace AiSqlEngine.Core.Services;

internal sealed class AiQueryService : IAiQueryService
{
    private readonly ISqlValidator _sqlValidator;
    private readonly ISqlExecutor _sqlExecutor;
    private readonly IDbSchemaTool _dbSchemaTool;
    private readonly ISqlBuilder _sqlBuilder;
    private readonly IPromptBuilder _promptBuilder;
    private readonly ILlmClient _llmClient;

    public AiQueryService(ISqlValidator sqlValidator,
                          ISqlExecutor sqlExecutor,
                          IDbSchemaTool dbSchemaTool,
                          ISqlBuilder sqlBuilder,
                          IPromptBuilder promptBuilder,
                          ILlmClient llmClient)
    {
        _sqlValidator = sqlValidator;
        _sqlExecutor = sqlExecutor;
        _dbSchemaTool = dbSchemaTool;
        _sqlBuilder = sqlBuilder;
        _promptBuilder = promptBuilder;
        _llmClient = llmClient;
    }

    public async Task<Result<string>> ConvertToSqlAsync(string message, CancellationToken cancellationToken)
    {
        var schema = await _dbSchemaTool.GetSchemaAsync(cancellationToken);

        if (schema.IsFailed)
        {
            return Result.Fail<string>(schema.Errors);
        }

        var promptMessage = _promptBuilder.BuildPrompt(message, schema.Value);
        var queryPlan = await _llmClient.GenerateAsync(promptMessage, cancellationToken);

        if (queryPlan.IsFailed)
        {
            return Result.Fail<string>(queryPlan.Errors);
        }

        var sqlResult = _sqlBuilder.Build(queryPlan.Value);
        
        if (sqlResult.IsFailed)
        {
            return Result.Fail<string>(sqlResult.Errors);
        }

        var validationResult = _sqlValidator.Validate(sqlResult.Value);
        
        if (validationResult.IsFailed)
        {
            return Result.Fail<string>(validationResult.Errors);
        }

        return sqlResult;
    }

    public async Task<Result<string>> ExecuteAsync(string sql, CancellationToken cancellationToken)
    {
        var validationResult = _sqlValidator.Validate(sql);
        
        if (validationResult.IsFailed)
        {
            return Result.Fail<string>(validationResult.Errors);
        }

        return await _sqlExecutor.ExecuteAsync(sql, cancellationToken);
    }
}