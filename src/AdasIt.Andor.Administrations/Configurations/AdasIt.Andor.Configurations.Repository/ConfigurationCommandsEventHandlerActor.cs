using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using Akka.Actor;
using Akka.Persistence;
using Mapster;

namespace AdasIt.Andor.Configurations.InfrastructureCommands;
public class ConfigurationCommandsEventHandlerActor : ReceivePersistentActor
{
    private readonly Guid _configId;
    private ConfigurationEntity? _configuration;

    public override string PersistenceId => $"configuration-{_configId}";

    public ConfigurationCommandsEventHandlerActor(Guid configId)
    {
        _configId = configId;

        Command<LoadConfiguration>(HandleLoadConfiguration);
        Command<ConfigurationCreated>(HandleConfigurationCreated);
        Command<ConfigurationUpdated>(HandleUpdateConfiguration);

        Recover<ConfigurationCreated>(Apply);
        Recover<ConfigurationUpdated>(Apply);
    }

    private void HandleLoadConfiguration(LoadConfiguration evt)
    {
        if (_configuration == null)
        {
            Context.Sender.Tell(null);

            return;
        }

        var configuration = _configuration.Adapt<Configuration>();

        Context.Sender.Tell(configuration);
    }

    private void HandleConfigurationCreated(ConfigurationCreated evt)
    {
        if (_configuration != null)
        {
            Sender.Tell(new Status.Failure(new InvalidOperationException("Configuration already exists.")));
            return;
        }

        Persist(evt, e =>
        {
            Apply(e);
            Sender.Tell(new Status.Success($"Configuration {_configId} created."));
        });
    }

    private void HandleUpdateConfiguration(ConfigurationUpdated evt)
    {
        if (_configuration == null)
        {
            Sender.Tell(new Status.Failure(new InvalidOperationException("Configuration not found.")));
            return;
        }

        Persist(evt, e =>
        {
            Apply(e);
            Sender.Tell(new Status.Success($"Configuration {_configId} updated."));
        });
    }

    private void Apply(ConfigurationCreated evt)
    {
        _configuration = evt.Adapt(_configuration);
    }

    private void Apply(ConfigurationUpdated evt)
    {
        if (_configuration != null)
        {
            _configuration = evt.Adapt(_configuration);
        }
    }
}

