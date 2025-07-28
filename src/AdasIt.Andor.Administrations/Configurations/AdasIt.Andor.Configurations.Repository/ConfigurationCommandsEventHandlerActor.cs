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
    {
        var propertyValues = new Dictionary<string, object>
        {
            { nameof(Configuration.Id), (ConfigurationId)@base.Id },
            { nameof(Configuration.Name), @base.Name },
            { nameof(Configuration.Value), @base.Value },
            { nameof(Configuration.Description), @base.Description },
            { nameof(Configuration.CreatedBy), @base.CreatedBy },
            { nameof(Configuration.StartDate), @base.StartDate },
            { nameof(Configuration.CreatedAt), @base.CreatedAt }
         };

        if (@base.ExpireDate != null)
        {
            propertyValues.Add(nameof(Configuration.ExpireDate), @base.ExpireDate);
        }

        return CreateInstanceAndSetProperties<Configuration>(propertyValues);
    }
    public static T CreateInstanceAndSetProperties<T>(Dictionary<string, object> propertyValues) where T : class
    {
        Type type = typeof(T);

        var instance = (T)Activator.CreateInstance(type, true);

        foreach (var property in typeof(T).GetProperties())
        {
            if (propertyValues.TryGetValue(property.Name, out var value))
            {
                property.SetValue(instance, value);
            }
        }

        return instance;
    }
}

