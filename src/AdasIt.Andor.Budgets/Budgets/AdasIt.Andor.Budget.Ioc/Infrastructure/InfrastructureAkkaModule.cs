using AdasIt.Andor.Budgets.InfrastructureCommands;
using AdasIt.Andor.Infrastructure;
using Akka.Configuration;
using Akka.DependencyInjection;
using Akka.Hosting;
using Microsoft.Extensions.Configuration;

namespace AdasIt.Andor.Budget.Ioc.Infrastructure;

internal class InfrastructureAkkaModule(IConfiguration configuration) : IAkkaModule
{
    private readonly string ConnectionString = configuration["Akka:Persistence:ConnectionString"] ?? "";

    public void Configure(AkkaConfigurationBuilder builder, IServiceProvider provider)
    {
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
                    connection-string = ""{ConnectionString}""
                    schema-name = ""public""
                    table-name = ""event_journal""
                    auto-initialize = true
                }}
                snapshot-store.plugin = ""akka.persistence.snapshot-store.postgresql""
                snapshot-store.postgresql {{
                    class = ""Akka.Persistence.PostgreSql.Snapshot.PostgreSqlSnapshotStore, Akka.Persistence.PostgreSql""
                    plugin-dispatcher = ""akka.actor.default-dispatcher""
                    connection-string = ""{ConnectionString}""
                    schema-name = ""public""
                    table-name = ""snapshot_store""
                    auto-initialize = true
                }}
            }}
        }}
    ");

        builder.AddHocon(akkaConfig, HoconAddMode.Prepend);

        builder.WithActors((system, registry) =>
        {
            var props = DependencyResolver.For(system)
                            .Props<AccountCommandsInfrastructureSupervisor>();

            var supervisor = system.ActorOf(props, nameof(AccountCommandsInfrastructureSupervisor));
            registry.Register<AccountCommandsInfrastructureSupervisor>(supervisor);
        });
    }
}
