using AdasIt.Andor.ApplicationDto.Commands;
using AdasIt.Andor.Budgets.ApplicationDto;
using Akka.Actor;

namespace AdasIt.Andor.Budget.Application.Actors;

public class AccountManagerActor : ReceiveActor
{
    public AccountManagerActor(IServiceProvider serviceProvider)
    {
        Receive<CreateAccount>(cmd =>
        {
            Handler(serviceProvider, cmd);
        });
    }

    private static void Handler(IServiceProvider serviceProvider, ICommands<Guid> cmd)
    {
        var childName = $"{nameof(AccountActor)}-{cmd.Id.ToString()}";

        var child = Context.Child(childName);

        if (child.Equals(ActorRefs.Nobody))
        {
            child = Context.ActorOf(Props.Create(
                () => new AccountActor(cmd.Id, serviceProvider)), childName);
        }

        child.Forward(cmd);
    }
}
