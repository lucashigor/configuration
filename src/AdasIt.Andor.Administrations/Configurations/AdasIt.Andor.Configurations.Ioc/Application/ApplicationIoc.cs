using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Ioc.Application;

internal static class ApplicationIoc
{
    public static IServiceCollection UseApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAkkaModule, ApplicationAkkaModule>();

        services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();

        services.AddScoped<IConfigurationQueriesService, ConfigurationQueriesService>();

        services.AddScoped<IConfigurationCommandsService, ConfigurationCommandsService>();

        return services;
    }
}
