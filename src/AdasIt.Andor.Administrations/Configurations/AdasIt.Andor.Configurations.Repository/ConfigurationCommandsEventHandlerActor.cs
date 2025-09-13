using Adasit.Andor.Mapping;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using Akka.Actor;
using Akka.Persistence;

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

        var configuration = Mappings.GetValid<Configuration, ConfigurationEntity>(_configuration);

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
        if (_configuration == null)
        {
            _configuration = new ConfigurationEntity();
        }

        _configuration.Id = evt.Id;
        _configuration.Name = evt.Name;
        _configuration.Value = evt.Value;
        _configuration.Description = evt.Description;
        _configuration.StartDate = evt.StartDate;
        _configuration.ExpireDate = evt.ExpireDate;
        _configuration.CreatedBy = evt.CreatedBy;
        _configuration.CreatedAt = evt.CreatedAt;
    }

    private void Apply(ConfigurationUpdated evt)
    {
        if (_configuration != null)
        {
            _configuration.Id = evt.Id;
            _configuration.Name = evt.Name;
            _configuration.Value = evt.Value;
            _configuration.Description = evt.Description;
            _configuration.StartDate = evt.StartDate;
            _configuration.ExpireDate = evt.ExpireDate;
        }
    }
}

