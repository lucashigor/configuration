using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Infrastructure;
using Akka.Configuration;
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
                var props = DependencyResolver.For(system).Props<ConfigurationManagerActor>();
                var actorRef = system.ActorOf(props, "configuration-manager-actor");
                registry.Register<ConfigurationManagerActor>(actorRef);
            });

            akka.WithActors((system, registry) =>
            {
                var props = DependencyResolver.For(system).Props<ConfigurationProjectionActor>();
                var actorRef = system.ActorOf(props, "configuration-projection-actor");
                registry.Register<ConfigurationProjectionActor>(actorRef);
            });
        });

        return services;
    }
}
