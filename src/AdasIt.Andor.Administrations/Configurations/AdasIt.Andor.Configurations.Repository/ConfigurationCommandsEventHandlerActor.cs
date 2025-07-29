using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Infrastructure.Config;
using Akka.Actor;
using Akka.Persistence;
using Mapster;

namespace AdasIt.Andor.Configurations.Repository;
public class ConfigurationCommandsEventHandlerActor : ReceivePersistentActor
{
    private readonly Guid _configId;
    private ConfigurationDto? _configuration;

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

        var configuration = LoadConfiguration(_configuration!);

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

    public static Configuration LoadConfiguration(ConfigurationDto @base)
        => Configuration.Load(new ConfigurationId(@base.Id), @base.Name, @base.Value,
            @base.Description, @base.StartDate, @base.ExpireDate, @base.CreatedBy,
            @base.CreatedAt);
}

