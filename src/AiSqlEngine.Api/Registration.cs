using AiSqlEngine.Core;
using AiSqlEngine.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AiSqlEngine.Api;

public static class Registration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddCore();
        services.AddInfrastructure();

        return services;
    }
}