using AiSqlEngine.Core.Builders;
using AiSqlEngine.Core.Interfaces;
using AiSqlEngine.Core.Services;
using AiSqlEngine.Core.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace AiSqlEngine.Core;

public static class Registration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IAiQueryService, AiQueryService>();
        services.AddScoped<IQueryPlanValidator, QueryPlanValidator>();
        services.AddScoped<IPromptBuilder, PromptBuilder>();

        return services;
    }
}