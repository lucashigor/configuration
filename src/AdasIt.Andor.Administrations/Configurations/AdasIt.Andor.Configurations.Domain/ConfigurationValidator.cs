using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public class ConfigurationValidator(DomainQueries.IQueriesConfigurationRepository query)
    : DefaultValidator<Configuration, ConfigurationId>, IConfigurationValidator
{
    public override async Task<List<Notification>> ValidateCreationAsync(Configuration entity, 
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = [];

        if (entity.StartDate < DateTimeOffset.UtcNow.AddSeconds(-5))
        {
            AddNotification(new Notification(nameof(entity.StartDate), 
                $"{nameof(entity.StartDate)} should be greater than now", CommonErrorCodes.Validation), 
                notifications);
        }

        if (entity.ExpireDate.HasValue && entity.ExpireDate < entity.StartDate)
        {
            AddNotification(new Notification(nameof(entity.ExpireDate),
                DefaultsErrorsMessages.Date0CannotBeBeforeDate1.GetMessage(nameof(entity.ExpireDate), 
                    nameof(entity.StartDate)), CommonErrorCodes.Validation), notifications);
        }

        if (entity.ExpireDate.HasValue && entity.ExpireDate < DateTimeOffset.UtcNow.AddSeconds(-5))
        {
            AddNotification(new Notification(nameof(entity.ExpireDate), 
                $"{nameof(entity.ExpireDate)} should be greater than now", CommonErrorCodes.Validation),
                notifications);
        }

        await DefaultValidationsAsync(entity, notifications, cancellationToken);

        return notifications;
    }

    public async Task<List<Notification>> ValidateUpdateAsync(Configuration existing, string name, string value, 
        string description, DateTime startDate, DateTime? expireDate, CancellationToken cancellationToken)
    {
        var notifications = new List<Notification>();

        var StartDateHasChanges = existing.StartDate.Equals(startDate) is false;
        var ExpireDateHasChanges = existing.ExpireDate.Equals(expireDate) is false;
        var NameHasChanges = existing.Name.Equals(name) is false;
        var ValueHasChanges = existing.Value.Equals(value) is false;

        if (existing.State.Equals(ConfigurationState.Expired) &&
                (StartDateHasChanges
                || ExpireDateHasChanges
                || NameHasChanges
                || ValueHasChanges))
        {
            var message = "only description are allowed to change on expired configuration";

            AddNotification(new(nameof(existing.ExpireDate), message, 
                ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange), notifications);
        }

        if (existing.State.Equals(ConfigurationState.Active) &&
            (NameHasChanges
            || StartDateHasChanges
            || ValueHasChanges))
        {
            var message = "it is not allowed to change name on active configuration";

            AddNotification(new(nameof(existing.StartDate), message, 
                ConfigurationsErrorCodes.ErrorOnChangeName), notifications);
        }

        await DefaultValidationsAsync(existing.Id.Value, name, value, description, startDate, expireDate, 
            notifications, cancellationToken);

        return notifications;
    }

    protected sealed override async Task DefaultValidationsAsync(Configuration entity, List<Notification> notifications, 
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        await DefaultValidationsAsync(entity.Id.Value, entity.Name, entity.Value, entity.Description,
            entity.StartDate, entity.ExpireDate, notifications, cancellationToken);
    }

    private async Task DefaultValidationsAsync(Guid id, string name, string value, string description,
        DateTime startDate, DateTime? expireDate, List<Notification> notifications, CancellationToken cancellationToken)
    {
        AddNotification(startDate.NotDefaultDateTime(), notifications);
        AddNotification(expireDate.NotDefaultDateTime(), notifications);
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 100), notifications);
        AddNotification(value.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(value.BetweenLength(1, 2500), notifications);
        AddNotification(description.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(description.BetweenLength(3, 1000), notifications);

        var listWithSameName = await query.GetByNameAndStatesAsync(
            new DomainQueries.SearchConfigurationInput()
            {
                Name = name,
                States = [DomainQueries.ConfigurationState.Active, DomainQueries.ConfigurationState.Awaiting]
            }, cancellationToken);

        if (listWithSameName is not null && listWithSameName.Exists(x => x.Id != id))
        {
            if (listWithSameName.Exists(x => x.StartDate <= startDate
                    && (x.ExpireDate == null || (x.ExpireDate != null && x.ExpireDate >= startDate))
                    && x.Id != id))
            {
                AddNotification(
                    new Notification("", ConfigurationsErrorCodes.ThereWillCurrentConfigurationStartDate)
                    , notifications);
            }

            if (listWithSameName.Exists(x => x.StartDate <= expireDate
                    && (x.ExpireDate == null || (x.ExpireDate != null && x.ExpireDate >= expireDate))
                    && x.Id != id))
            {
                AddNotification(
                    new Notification("", ConfigurationsErrorCodes.ThereWillCurrentConfigurationStartDate)
                    , notifications);
            }
        }
    }
}
