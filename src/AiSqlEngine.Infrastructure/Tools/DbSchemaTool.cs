using AiSqlEngine.Core.Interfaces;
using Dapper;
using System.Data;
using FluentResults;

namespace AiSqlEngine.Infrastructure.Tools;

internal sealed class DbSchemaTool : IDbSchemaTool
{
    private readonly IDbConnection _dbConnection;

    public DbSchemaTool(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<string>> GetSchemaAsync(CancellationToken cancellationToken)
    {
        try
        {
            var tables = await _dbConnection.QueryAsync<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");

            var schemaParts = new List<string>();

            foreach (var table in tables)
            {
                var columns = await _dbConnection.QueryAsync("SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table", new { table });

                var cols = string.Join(", ", columns.Select(c => $"{c.COLUMN_NAME} ({c.DATA_TYPE})"));

                schemaParts.Add($"Table: {table}\nColumns: {cols}");
            }
                
            return Result.Ok(string.Join("\n\n", schemaParts));
        }
        catch (Exception e)
        {
            return Result.Fail<string>($"Could not obtain schema from DB. Error: {e}");
        }
            
    }
}