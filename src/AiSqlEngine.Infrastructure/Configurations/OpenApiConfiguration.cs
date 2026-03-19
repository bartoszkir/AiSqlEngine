namespace AiSqlEngine.Infrastructure.Configurations;

public sealed record  OpenApiConfiguration
{
    public const string SectionName = "OpenAI";

    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string? Deployment { get; set; }
}