using AiSqlEngine.Core.Interfaces;
using AiSqlEngine.Core.Models;
using FluentResults;

namespace AiSqlEngine.Infrastructure.Builders;

internal sealed class SqlBuilder : ISqlBuilder
{
    public Result<string> Build(QueryPlan plan)
    {
        try
        {
            if (plan == null)
                return Result.Fail<string>("Query plan cannot be null.");

            if (plan.Select == null || !plan.Select.Any())
                return Result.Fail<string>("SELECT clause is required and cannot be empty.");

            if (string.IsNullOrWhiteSpace(plan.From))
                return Result.Fail<string>("FROM clause is required.");

            var select = string.Join(", ", plan.Select);

            var topClause = plan.Limit.HasValue
                ? $"TOP ({plan.Limit.Value}) "
                : string.Empty;

            var sql = $"SELECT {topClause}{select} FROM {plan.From}";
            
            if (plan.Joins != null && plan.Joins.Any())
            {
                foreach (var join in plan.Joins)
                {
                    var type = (join.Type ?? "INNER").ToUpperInvariant();

                    if (string.IsNullOrWhiteSpace(join.Table))
                        return Result.Fail<string>("Join table cannot be empty.");

                    if (string.IsNullOrWhiteSpace(join.On))
                        return Result.Fail<string>("Join ON condition cannot be empty.");

                    sql += $" {type} JOIN {join.Table} ON {join.On}";
                }
            }

            if (!string.IsNullOrWhiteSpace(plan.Where))
                sql += $" WHERE {plan.Where}";
            
            if (plan.GroupBy != null && plan.GroupBy.Any())
                sql += $" GROUP BY {string.Join(", ", plan.GroupBy)}";
            
            if (plan.OrderBy != null && plan.OrderBy.Any())
                sql += $" ORDER BY {string.Join(", ", plan.OrderBy)}";

            return Result.Ok(sql);
        }
        catch (Exception e)
        {
            return Result.Fail<string>($"Failed to build SQL query: {e.Message}");
        }
    }
}