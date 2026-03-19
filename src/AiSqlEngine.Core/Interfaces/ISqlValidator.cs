using FluentResults;

namespace AiSqlEngine.Core.Interfaces;

public interface ISqlValidator
{
    Result Validate(string sql);
}