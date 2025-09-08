using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc.Infrastructure;

public static class InfrastructureIoc
{
    internal static IServiceCollection UseInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.UseDbContext(configuration);
        
        return services;
    }
}