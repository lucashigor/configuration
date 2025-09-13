using AdasIt.Andor.Budget.Application.Actors;
using AdasIt.Andor.Infrastructure;
using Akka.DependencyInjection;
using Akka.Hosting;

namespace AdasIt.Andor.Budget.Ioc.Application;


internal class ApplicationAkkaModule : IAkkaModule
{
    public void Configure(AkkaConfigurationBuilder builder, IServiceProvider provider)
    {
        builder.WithActors((system, registry) =>
        {
            var props = DependencyResolver.For(system)
                            .Props<AccountManagerActor>();

            var actorRef = system.ActorOf(props, nameof(AccountManagerActor));
            registry.Register<AccountManagerActor>(actorRef);
        });
    }
}