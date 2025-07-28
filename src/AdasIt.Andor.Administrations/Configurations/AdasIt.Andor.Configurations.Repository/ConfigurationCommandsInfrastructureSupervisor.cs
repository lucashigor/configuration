using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Repository;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.InfrastructureCommands;

public class ConfigurationCommandsInfrastructureSupervisor : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;

    public ConfigurationCommandsInfrastructureSupervisor(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;

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

        Receive<LoadConfiguration>(cmd =>
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

    private static IActorRef GetActor(Guid id)
    {
        var childName = $"configuration-commands-{id.ToString()}";

        var child = Context.Child(childName);

        if (child == ActorRefs.Nobody)
        {
            child = Context.ActorOf(Props.Create(() =>
                        new ConfigurationCommandsEventHandlerActor(id)), childName);
        }

        return child;
    }
}
