using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Domain;
using Akka.DependencyInjection;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace AdasIt.Andor.Configurations.WebApi;

public static class ConfigurationExtensions
{
    public static IServiceCollection UseConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ConfigurationController).Assembly));

        services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();

        services.AddAkka("ConfigSystem", (akka, provider) =>
        {
            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system).Props<ConfigurationManagerActor>();
                var actorRef = system.ActorOf(props, "configuration-manager-actor");
                registry.Register<ConfigurationManagerActor>(actorRef);
            });
        });

        return services;
    }
}
