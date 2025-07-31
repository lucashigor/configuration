using AdasIt.Andor.Configurations.Ioc.Application;
using AdasIt.Andor.Configurations.Ioc.Infrastructure;
using AdasIt.Andor.Configurations.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Ioc;

public static class ConfigurationExtensions
{
    public static IServiceCollection UseConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseApi(configuration)
            .UseApplication()
            .UseInfrastructure(configuration);

        return services;
    }
}
