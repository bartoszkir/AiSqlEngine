using AiSqlEngine.Core.Models;

namespace AiSqlEngine.Core.Validators;

internal interface IQueryPlanValidator
{
    void Validate(QueryPlan? plan);
}

internal sealed class QueryPlanValidator : IQueryPlanValidator
{
    public void Validate(QueryPlan? plan)
    {
        if (plan is null)
        {
            throw new Exception("Plan is null");
        }

        if (plan.From == null)
            throw new Exception("Table required");

        if (plan.Select.Count == 0)
            throw new Exception("Select required");
    }
}