using AdasIt.Andor.Budgets.InfrastructureQueries.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc.Infrastructure;
internal static class InfrastructureDbContext
{
    internal static IServiceCollection UseDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString(nameof(BudgetContext));

        if (string.IsNullOrEmpty(conn) is true) return services;
        
        services.AddDbContext<BudgetContext>(options =>
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
        var db = scope.ServiceProvider.GetRequiredService<BudgetContext>();
        db.Database.Migrate();

        return services;
    }
}