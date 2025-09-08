using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public class Configuration : AggregateRoot<ConfigurationId>
{
    public Name Name { get; private set; }
    public Value Value { get; private set; }
    public Description Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ConfigurationState State => GetStatus(false, StartDate, ExpireDate);

    public Configuration()
    {
            
    }
    
    private Configuration(
            ConfigurationId id,
            Name name,
            Value value,
            Description description,
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
        Name name,
        Value value,
        Description description,
        DateTime startDate,
        DateTime? expireDate,
        string userId,
        IConfigurationValidator configurationValidator,
        CancellationToken cancellationToken)
        => NewAsync(ConfigurationId.New(), name, value, description, startDate, expireDate, userId, 
            configurationValidator, cancellationToken);

    public static async Task<(DomainResult, Configuration?)> NewAsync(
        ConfigurationId id,
        Name name,
        Value value,
        Description description,
        DateTime startDate,
        DateTime? expireDate,
        string userId,
        IConfigurationValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Configuration(
            id,
            name,
            value,
            description,
            startDate,
            expireDate,
            userId,
            DateTime.UtcNow);

        return await entity.ValidateAsync(validator, cancellationToken);
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
    public async Task<DomainResult> UpdateAsync(Name name, Value value, Description description, DateTime startDate, 
        DateTime? expireDate, IConfigurationValidator configurationValidator, CancellationToken cancellationToken)
    {
        var notifications = await configurationValidator.ValidateUpdateAsync(this, name, 
            value, description, startDate, expireDate, cancellationToken);

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

