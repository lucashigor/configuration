using AdasIt.Andor.Budgets.Domain.Accounts.Commands;
using AdasIt.Andor.Budgets.Domain.Accounts.Events;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Budgets.InfrastructureCommands;

public class AccountCommandsInfrastructureSupervisor : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;

    public AccountCommandsInfrastructureSupervisor(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;

        Receive<AccountCreated>(x => Handler(x.Id, x));

        Receive<LoadAccount>(x => Handler(x.Id, x));
    }

    protected override void PreStart()
    {
        _eventPublisher.SubscribeAsync(Self.Tell, CancellationToken.None).GetAwaiter().GetResult();
    }

    private static void Handler(Guid Id, object cmd)
    {
        var child = GetActor(Id);

        child.Forward(cmd);
    }

    private static IActorRef GetActor(Guid id)
    {
        var childName = $"{nameof(AccountCommandsEventHandlerActor)}-{id}";

        var child = Context.Child(childName);

        if (child.Equals(ActorRefs.Nobody))
        {
            child = Context.ActorOf(Props.Create(() =>
                        new AccountCommandsEventHandlerActor(id)), childName);
        }

        return child;
    }
}
