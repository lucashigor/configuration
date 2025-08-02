using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public class Configuration : AggregateRoot<ConfigurationId>
{
    public string Name { get; private set; }
    public string Value { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ConfigurationState State => GetStatus(false, StartDate, ExpireDate);

    private Configuration(
            ConfigurationId id,
            string name,
            string value,
            string description,
            DateTime startDate,
            DateTime? expireDate,
            string userId,
            DateTime createdAt)
    {
        Id = id;
        Name = name;
        Value = value;
        Description = description;
        StartDate = startDate;
        ExpireDate = expireDate;
        CreatedBy = userId;
        CreatedAt = createdAt;
    }

    public static Task<(DomainResult, Configuration?)> NewAsync(
        string name,
        string value,
        string description,
        DateTime startDate,
        DateTime? expireDate,
        string userId,
        IConfigurationValidator configurationValidator,
        CancellationToken cancellationToken)
        => NewAsync(ConfigurationId.New(), name, value, description, startDate, expireDate, userId, configurationValidator
            , cancellationToken);

    public static async Task<(DomainResult, Configuration?)> NewAsync(
        ConfigurationId Id,
        string name,
        string value,
        string description,
        DateTime startDate,
        DateTime? expireDate,
        string userId,
        IConfigurationValidator configurationValidator,
        CancellationToken cancellationToken)
    {
        var entity = new Configuration(
            Id,
            name,
            value,
            description,
            startDate,
            expireDate,
            userId,
            DateTime.UtcNow);

        var notifications = await configurationValidator.ValidateCreationAsync(name, value, description, startDate, expireDate, cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        entity.RaiseDomainEvent(ConfigurationCreated.FromConfiguration(entity));

        return (result, entity);
    }

    public static Configuration Load(
    ConfigurationId id,
    string name,
    string value,
    string description,
    DateTime startDate,
    DateTime? expireDate,
    string createdBy,
    DateTime createdAt)
    {
        return new Configuration(id, name, value, description, startDate, expireDate, createdBy, createdAt);
    }

    public static ConfigurationState GetStatus(bool _isDeleted, DateTime _startDate, DateTime? _expireDate)
    {
        if (_isDeleted)
        {
            return ConfigurationState.Deleted;
        }

        if (_startDate > DateTime.UtcNow)
        {
            return ConfigurationState.Awaiting;
        }

        if (_startDate < DateTime.UtcNow && (_expireDate.HasValue is false || _expireDate.Value > DateTime.UtcNow))
        {
            return ConfigurationState.Active;
        }

        if (_expireDate.HasValue && _expireDate.Value < DateTime.UtcNow)
        {
            return ConfigurationState.Expired;
        }

        return ConfigurationState.Undefined;
    }

    #region Update
    public async Task<DomainResult> UpdateAsync(string name, string value, string description, DateTime startDate, DateTime? expireDate,
        IConfigurationValidator configurationValidator, CancellationToken cancellationToken)
    {
        var notifications = await configurationValidator.ValidateUpdateAsync(this, name, value, description, startDate, expireDate,
            cancellationToken);

        AddNotification(notifications);

        var result = Validate();

        if (result.IsFailure)
        {
            return result;
        }

        Name = name;
        Value = value;
        Description = description;
        StartDate = startDate;
        ExpireDate = expireDate;

        RaiseDomainEvent(ConfigurationUpdated.FromConfiguration(this));

        return result;
    }

    #endregion

    public async Task<DomainResult> DeactivateAsync(IConfigurationValidator configurationValidator,
        CancellationToken cancellationToken)
    {
        if (State == ConfigurationState.Expired)
        {
            AddNotification(nameof(ExpireDate),
                "not allowed to Deactivate expired configurations",
                ConfigurationsErrorCodes.ErrorOnDeleteConfiguration);
        }

        if (State == ConfigurationState.Active)
        {
            await UpdateAsync(Name, Value, Description, StartDate, DateTime.UtcNow, configurationValidator,
                cancellationToken);

            AddWarning(nameof(ExpireDate),
                "expire date set to today",
                ConfigurationsErrorCodes.SetExpireDateToToday);

            RaiseDomainEvent(ConfigurationDeactivated.FromConfiguration(this));
        }

        if (State == ConfigurationState.Awaiting)
        {
            AddNotification(nameof(ExpireDate),
                "not allowed to Deactivate expired configurations, try Deleting",
                ConfigurationsErrorCodes.ErrorOnDeleteConfiguration);
        }

        return Validate();
    }


    public DomainResult Delete()
    {
        if (State == ConfigurationState.Expired)
        {
            AddNotification(nameof(ExpireDate),
                "not allowed to delete expired configurations",
                ConfigurationsErrorCodes.ErrorOnDeleteConfiguration);
        }

        if (State == ConfigurationState.Awaiting)
        {
            RaiseDomainEvent(ConfigurationDeleted.FromConfiguration(this));
        }

        return Validate();
    }
}

