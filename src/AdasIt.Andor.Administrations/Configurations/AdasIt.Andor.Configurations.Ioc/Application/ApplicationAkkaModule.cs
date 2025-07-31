using AdasIt.Andor.Configurations.Application.Actors;
using AdasIt.Andor.Infrastructure;
using Akka.DependencyInjection;
using Akka.Hosting;

namespace AdasIt.Andor.Configurations.Ioc.Application;

internal class ApplicationAkkaModule : IAkkaModule
{
    public void Configure(AkkaConfigurationBuilder builder, IServiceProvider provider)
    {
        builder.WithActors((system, registry) =>
        {
            var props = DependencyResolver.For(system)
                            .Props<ConfigurationManagerActor>();

            var actorRef = system.ActorOf(props, nameof(ConfigurationManagerActor));
            registry.Register<ConfigurationManagerActor>(actorRef);
        });
    }
}
