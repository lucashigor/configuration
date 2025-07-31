using AdasIt.Andor.Configurations.Application.Actions;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.Application.Actors;

public class ConfigurationManagerActor : ReceiveActor
{
    public ConfigurationManagerActor(IServiceProvider serviceProvider)
    {
        Receive<CreateConfiguration>(cmd =>
        {
            Handler(serviceProvider, cmd);
        });
        
        Receive<UpdateConfiguration>(cmd =>
        {
            Handler(serviceProvider, cmd);
        });
    }

    private static void Handler(IServiceProvider serviceProvider, IAction<ConfigurationId> cmd)
    {
        var childName = $"{nameof(ConfigurationActor)}-{cmd.Id.ToString()}";

        var child = Context.Child(childName);

        if (child.Equals(ActorRefs.Nobody))
        {
            child = Context.ActorOf(Props.Create(
                () => new ConfigurationActor(cmd.Id, serviceProvider)), childName);
        }

        child.Forward(cmd);
    }
}
