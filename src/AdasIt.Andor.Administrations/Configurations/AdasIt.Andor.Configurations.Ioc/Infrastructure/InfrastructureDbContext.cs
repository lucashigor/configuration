using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Ioc.Infrastructure;

internal static class InfrastructureDbContext
{
    internal static IServiceCollection UseDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString(nameof(ConfigurationContext));

        if (string.IsNullOrEmpty(conn) is false)
        {
            services.AddDbContext<ConfigurationContext>(options =>
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();

                options.UseNpgsql(conn, x =>
                {
                    x.EnableRetryOnFailure(5);
                    x.MinBatchSize(1);
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
            db.Database.Migrate();
        }

        return services;
    }
}
