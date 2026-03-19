
namespace AiSqlEngine.Infrastructure.Configurations;

public sealed record ConnectionStringsConfiguration
{
    public const string SectionName = "ConnectionStrings";

    public string? DatabaseConnection { get; set; }
}