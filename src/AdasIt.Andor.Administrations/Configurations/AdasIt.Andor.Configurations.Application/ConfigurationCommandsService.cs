using AdasIt.Andor.Configurations.Application.Actors;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Dto;
using Akka.Actor;
using Akka.Hosting;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationCommandsService : IConfigurationCommandsService
{
    private readonly IActorRef _configActor;

    private TimeSpan Timeout =>
#if DEBUG
        TimeSpan.FromHours(2);
#else
    TimeSpan.FromSeconds(5);
#endif

    public ConfigurationCommandsService(ActorRegistry registry)
    {
        _configActor = registry.Get<ConfigurationManagerActor>();
    }

    public Task CreateConfigurationAsync(CreateConfiguration command, CancellationToken cancellationToken)
    {
        command.CancellationToken = cancellationToken;

        _configActor.Tell(command);

        return Task.CompletedTask;
    }

    public Task DeleteConfigurationAsync(Guid configurationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateConfigurationAsync(UpdateConfiguration command, CancellationToken cancellationToken)
    {
        command.CancellationToken = cancellationToken;

        _configActor.Tell(command);

        return Task.CompletedTask;
    }
}
