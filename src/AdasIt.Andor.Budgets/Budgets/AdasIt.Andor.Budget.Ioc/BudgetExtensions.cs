using AdasIt.Andor.Budget.Ioc.Application;
using AdasIt.Andor.Budget.Ioc.Infrastructure;
using AdasIt.Andor.Budgets.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc;

public static class BudgetExtensions
{
    public static IServiceCollection UseBudget(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseApi(configuration)
            .UseApplication()
            .UseInfrastructure(configuration);

        return services;
    }
}