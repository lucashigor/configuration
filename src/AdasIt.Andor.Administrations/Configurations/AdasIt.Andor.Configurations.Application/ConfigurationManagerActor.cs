using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Dto;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationManagerActor : ReceiveActor
{
    public ConfigurationManagerActor(IServiceProvider _serviceProvider)
    {
        Receive<CreateConfiguration>(cmd =>
        {
            var configId = ConfigurationId.New();
            var child = Context.Child(configId.ToString())
                        ?? Context.ActorOf(Props.Create(() => new ConfigurationActor(configId, _serviceProvider)), configId.ToString());

            child.Forward(cmd);
        });
    }
}

