using AiSqlEngine.Core.Models;
using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface ISqlBuilder
{
    Result<string> Build(QueryPlan plan);
}