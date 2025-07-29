using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.InfrastructureQueries;

public class ConfigurationQueriesInfrastructureSupervisor : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IServiceProvider _serviceProvider;
    public ConfigurationQueriesInfrastructureSupervisor(IEventPublisher eventPublisher,
        IServiceProvider serviceProvider)
    {
        _eventPublisher = eventPublisher;
        _serviceProvider = serviceProvider;

        Receive<GetConfiguration>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });

        Receive<ConfigurationCreated>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });

        Receive<ConfigurationDeactivated>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });

        Receive<ConfigurationDeleted>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });

        Receive<ConfigurationUpdated>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });

        Receive<object>(evt =>
        {
            switch (evt)
            {
                case ConfigurationCreated created:
                case ConfigurationUpdated updated:
                case ConfigurationDeactivated deactivated:
                case ConfigurationDeleted deleted:
                    Self.Tell(evt);
                    break;

                default:
                    break;
            }
        });
    }

    protected override void PreStart()
    {
        _eventPublisher.SubscribeAsync(Self.Tell, CancellationToken.None).GetAwaiter().GetResult();
    }

    private IActorRef GetActor(Guid id)
    {
        var childName = $"configuration-queries-{id}";

        var child = Context.Child(childName);

        if (child == ActorRefs.Nobody)
        {
            child = Context.ActorOf(Props.Create(() =>
                        new ConfigurationQueriesEventHandler(id, _serviceProvider)), childName);
        }

        return child;
    }
}
