namespace AiSqlEngine.Core.Models;

public sealed record JoinClause
{
    public string Type { get; set; } = "INNER";
    public string Table { get; set; }
    public string On { get; set; }
}