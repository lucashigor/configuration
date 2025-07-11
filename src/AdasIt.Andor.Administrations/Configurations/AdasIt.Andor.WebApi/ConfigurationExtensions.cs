using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Infrastructure;
using AdasIt.Andor.Configurations.Infrastructure.Repositories;
using AdasIt.Andor.Configurations.Repository;
using Akka.DependencyInjection;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

namespace AdasIt.Andor.Configurations.WebApi;

public static class ConfigurationExtensions
{
    public static IServiceCollection UseConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ConfigurationController).Assembly));

        services.AddAutoMapper(typeof(MappingProfile));

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

        services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();

        services.AddAkka("ConfigSystem", (akka, provider) =>
        {
            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system).Props<ConfigurationActor>();
                var actorRef = system.ActorOf(props, "configuration-actor");
                registry.Register<ConfigurationActor>(actorRef);
            });

            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system).Props<ConfigurationEventHandlerActor>();
                var actorRef = system.ActorOf(props, "configuration-event-handler-actor");
                registry.Register<ConfigurationEventHandlerActor>(actorRef);
            });

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
