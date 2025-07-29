using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Application.Actors;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Repository;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.InfrastructureCommands;
using AdasIt.Andor.Configurations.InfrastructureQueries;
using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using AdasIt.Andor.Infrastructure;
using Akka.Configuration;
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

        services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();
        services.AddScoped<IConfigurationCommandsService, ConfigurationCommandsService>();
        services.AddScoped<IConfigurationQueriesService, ConfigurationQueriesService>();
        services.AddScoped<IQueriesConfigurationRepository<ConfigurationOutput>,
            QueriesConfigurationRepository>();

        var config = configuration["Akka:Persistence:ConnectionString"];

        var akkaConfig = ConfigurationFactory.ParseString($@"
        akka {{
            actor {{
                provider = ""Akka.Actor.LocalActorRefProvider""
                serializers {{hyperion = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""}}
                serialization-bindings {{""System.Object"" = hyperion}}
            }}
            persistence {{
                journal.plugin = ""akka.persistence.journal.postgresql""
                journal.postgresql {{
                    class = ""Akka.Persistence.PostgreSql.Journal.PostgreSqlJournal, Akka.Persistence.PostgreSql""
                    plugin-dispatcher = ""akka.actor.default-dispatcher""
                    connection-string = ""{config}""
                    schema-name = ""public""
                    table-name = ""event_journal""
                    auto-initialize = true
                }}
                snapshot-store.plugin = ""akka.persistence.snapshot-store.postgresql""
                snapshot-store.postgresql {{
                    class = ""Akka.Persistence.PostgreSql.Snapshot.PostgreSqlSnapshotStore, Akka.Persistence.PostgreSql""
                    plugin-dispatcher = ""akka.actor.default-dispatcher""
                    connection-string = ""{config}""
                    schema-name = ""public""
                    table-name = ""snapshot_store""
                    auto-initialize = true
                }}
            }}
        }}
    ");

        services.AddAkka("ConfigSystem", (akka, provider) =>
        {
            akka.AddHocon(akkaConfig, HoconAddMode.Prepend);

            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system)
                                .Props<ConfigurationManagerActor>();

                var actorRef = system.ActorOf(props, "configuration-manager-actor");
                registry.Register<ConfigurationManagerActor>(actorRef);
            });

            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system)
                                .Props<ConfigurationCommandsInfrastructureSupervisor>();

                var supervisor = system.ActorOf(props, "configuration-infrastructure-supervisor");
                registry.Register<ConfigurationCommandsInfrastructureSupervisor>(supervisor);
            });

            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system)
                                .Props<ConfigurationQueriesInfrastructureSupervisor>();

                var supervisor = system.ActorOf(props, "configuration-queries-event-handler-actor");
                registry.Register<ConfigurationQueriesInfrastructureSupervisor>(supervisor);
            });
        });

        services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

        services.AddSingleton<ICommandsConfigurationRepository>(sp =>
        {
            var supervisor = sp.GetRequiredService<ActorRegistry>().Get<ConfigurationCommandsInfrastructureSupervisor>();
            var eventPublisher = sp.GetRequiredService<IEventPublisher>();
            return new ConfigurationCommandRepository(supervisor, eventPublisher);
        });

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
