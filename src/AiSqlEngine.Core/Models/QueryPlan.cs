namespace AiSqlEngine.Core.Models;

public sealed record QueryPlan
{
    public string From { get; set; }
    public string? Where { get; set; }
    public List<string> Select { get; set; }
    public List<string>? GroupBy { get; set; }
    public List<string>? OrderBy { get; set; }
    public List<JoinClause>? Joins { get; set; }
    public int? Limit { get; set; }
}