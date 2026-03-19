using System.Data;
using AiSqlEngine.Core.Interfaces;
using Dapper;
using FluentResults;
using Newtonsoft.Json;

namespace AiSqlEngine.Infrastructure.Executors;

internal sealed class SqlExecutor : ISqlExecutor
{
    private readonly IDbConnection _dbConnection;

    public SqlExecutor(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<string>> ExecuteAsync(string sql, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _dbConnection.QueryAsync(sql);
            var json = JsonConvert.SerializeObject(data);
            return Result.Ok(json);
        }
        catch (Exception e)
        {
            return Result.Fail<string>($"Failed to execute SQL query: {e.Message}");
        }
    }
}