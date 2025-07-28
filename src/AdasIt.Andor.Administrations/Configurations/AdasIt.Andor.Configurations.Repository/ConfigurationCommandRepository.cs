using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.Repository;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Configurations.InfrastructureCommands;

public class ConfigurationCommandRepository : ICommandsConfigurationRepository
{
    private readonly IActorRef _supervisor;
    private readonly IEventPublisher _eventPublisher;

    public ConfigurationCommandRepository(IActorRef supervisor, IEventPublisher eventPublisher)
    {
        _supervisor = supervisor;
        _eventPublisher = eventPublisher;
    }

    public async Task<Configuration?> GetByIdAsync(ConfigurationId id, CancellationToken cancellationToken)
    {
        var result = await _supervisor.Ask<Configuration?>(new LoadConfiguration(id), cancellationToken: cancellationToken);

        return result;
    }

    public async Task PersistAsync(Configuration entity, CancellationToken cancellationToken)
    {
        var publishTasks = entity.Events
            .Select(@event => _eventPublisher.PublishAsync(@event, cancellationToken))
            .ToArray();

        await Task.WhenAll(publishTasks);

        entity.ClearEvents();
    }
}
