using AiSqlEngine.Core.Interfaces;
using FluentResults;

namespace AiSqlEngine.Infrastructure.Validators;

internal class SqlValidator : ISqlValidator
{
    private static readonly string[] Forbidden =
    {
        "INSERT",
        "UPDATE",
        "DELETE",
        "DROP",
        "ALTER",
        "TRUNCATE",
        "BEGIN",
        "COMMIT"
    };

    public Result Validate(string sql)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sql))
                return Result.Fail("SQL query cannot be null or empty.");

            var upper = sql.ToUpper();

            if (!upper.StartsWith("SELECT"))
                return Result.Fail("Only SELECT queries are allowed.");

            if (Forbidden.Any(upper.Contains))
                return Result.Fail("Forbidden SQL operation detected.");

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail($"SQL validation failed: {e.Message}");
        }
    }
}