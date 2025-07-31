using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Domain.Events;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.InfrastructureCommands;

public class ConfigurationCommandsInfrastructureSupervisor : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;

    public ConfigurationCommandsInfrastructureSupervisor(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;

        Receive<ConfigurationCreated>(Handler);

        Receive<ConfigurationDeactivated>(Handler);

        Receive<ConfigurationDeleted>(Handler);

        Receive<ConfigurationUpdated>(Handler);

        Receive<LoadConfiguration>(cmd =>
        {
            IActorRef child = GetActor(cmd.Id);

            child.Forward(cmd);
        });
    }

    protected override void PreStart()
    {
        _eventPublisher.SubscribeAsync(Self.Tell, CancellationToken.None).GetAwaiter().GetResult();
    }
    
    private static void Handler(DomainEvent cmd)
    {
        var child = GetActor(cmd.Id);

        child.Forward(cmd);
    }
    
    private static IActorRef GetActor(Guid id)
    {
        var childName = $"{nameof(ConfigurationCommandsEventHandlerActor)}-{id}";

        var child = Context.Child(childName);

        if (child.Equals(ActorRefs.Nobody))
        {
            child = Context.ActorOf(Props.Create(() =>
                        new ConfigurationCommandsEventHandlerActor(id)), childName);
        }

        return child;
    }
}
