using AdasIt.Andor.Configurations.Dto;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationManagerActor : ReceiveActor
{
    public ConfigurationManagerActor(IServiceProvider _serviceProvider)
    {
        Receive<CreateConfiguration>(cmd =>
        {
            var childName = $"configuration-{cmd.Id.ToString()}";

            var child = Context.Child(childName);

            if (child == ActorRefs.Nobody)
            {
                child = Context.ActorOf(Props.Create(() => new ConfigurationActor(cmd.Id, _serviceProvider)), childName);
            }

            child.Forward(cmd);
        });


        Receive<UpdateConfiguration>(cmd =>
        {
            var childName = $"configuration-{cmd.Id.ToString()}";

            var child = Context.Child(childName);

            if (child == ActorRefs.Nobody)
            {
                child = Context.ActorOf(Props.Create(() => new ConfigurationActor(cmd.Id, _serviceProvider)), childName);
            }

            child.Forward(cmd);
        });


        Receive<GetConfiguration>(cmd =>
        {
            var childName = $"configuration-{cmd.Id.ToString()}";

            var child = Context.Child(childName);

            if (child == ActorRefs.Nobody)
            {
                child = Context.ActorOf(Props.Create(() => new ConfigurationActor(cmd.Id, _serviceProvider)), childName);
            }

            child.Forward(cmd);
        });
    }
}

