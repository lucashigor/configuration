using AdasIt.Andor.Budget.Ioc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc;

public static class BudgetExtensions
{
    public static IServiceCollection UseBudget(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseInfrastructure(configuration);

        return services;
    }
}