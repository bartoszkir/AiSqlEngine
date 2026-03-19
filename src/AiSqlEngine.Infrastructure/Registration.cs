using AiSqlEngine.Core.Interfaces;
using AiSqlEngine.Infrastructure.Configurations;
using AiSqlEngine.Infrastructure.Executors;
using AiSqlEngine.Infrastructure.Validators;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using AiSqlEngine.Infrastructure.Builders;
using AiSqlEngine.Infrastructure.Clients;
using AiSqlEngine.Infrastructure.Tools;
using OpenAI;

namespace AiSqlEngine.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISqlValidator, SqlValidator>();
        services.AddScoped<ISqlExecutor, SqlExecutor>();
        services.AddScoped<IDbSchemaTool, DbSchemaTool>();
        services.AddScoped<ISqlBuilder, SqlBuilder>();
        services.AddScoped<ILlmClient, AzureOpenApiClient>();

        services.AddOptions<ConnectionStringsConfiguration>().BindConfiguration(ConnectionStringsConfiguration.SectionName);
        services.AddOptions<OpenApiConfiguration>().BindConfiguration(OpenApiConfiguration.SectionName);
        
        services.AddScoped<IDbConnection>(sp =>
        {
            var connectionStringsConfiguration = sp.GetService<IOptions<ConnectionStringsConfiguration>>();
            return new SqlConnection(connectionStringsConfiguration!.Value.DatabaseConnection);
        });

        return services;
    }
}