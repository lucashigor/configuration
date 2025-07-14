using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Infrastructure.Config;
using Akka.Actor;
using Mapster;

namespace AdasIt.Andor.Configurations.Infrastructure;

public class ConfigurationProjectionActor : ReceiveActor
{
    private readonly Dictionary<Guid, ConfigurationDto> _configs;
    public ConfigurationProjectionActor()
    {
        _configs = new Dictionary<Guid, ConfigurationDto>();

        Receive<ConfigurationCreated>(e =>
        {
            _configs[e.Id] = e.Adapt<ConfigurationDto>();
        });

        Receive<ConfigurationUpdated>(e =>
        {
            _configs[e.Id] = e.Adapt<ConfigurationDto>();
        });
    }
    protected override void PreStart()
    {
        Context.System.EventStream.Subscribe(Self, typeof(ConfigurationCreated));
        Context.System.EventStream.Subscribe(Self, typeof(ConfigurationUpdated));
    }
}

