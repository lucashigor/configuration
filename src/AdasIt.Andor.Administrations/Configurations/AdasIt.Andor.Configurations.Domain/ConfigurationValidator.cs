using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public class ConfigurationValidator : IConfigurationValidator
{
    private readonly DomainQueries.IQueriesConfigurationRepository _query;

    public ConfigurationValidator(DomainQueries.IQueriesConfigurationRepository query)
    {
        _query = query;
    }

    public async Task<List<Notification>> ValidateCreationAsync(string name, string value, string description, DateTime startDate, DateTime? expireDate,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        if (startDate < DateTimeOffset.UtcNow.AddSeconds(-5))
        {
            AddNotification(new Notification(nameof(startDate), $"{nameof(startDate)} should be greater than now", CommonErrorCodes.Validation), notifications);
        }

        if (expireDate.HasValue && expireDate < startDate)
        {
            AddNotification(new Notification(nameof(expireDate),
                DefaultsErrorsMessages.Date0CannotBeBeforeDate1.GetMessage(nameof(expireDate), nameof(startDate)),
                CommonErrorCodes.Validation), notifications);
        }

        if (expireDate.HasValue && expireDate < DateTimeOffset.UtcNow.AddSeconds(-5))
        {
            AddNotification(new Notification(nameof(expireDate), $"{nameof(expireDate)} should be greater than now", CommonErrorCodes.Validation), notifications);
        }

        await DefaultValidationsAsync(null, name, value, description, startDate, expireDate, notifications, cancellationToken);

        return notifications;
    }

    public async Task<List<Notification>> ValidateUpdateAsync(Configuration existing, string name, string value, string description, DateTime startDate,
        DateTime? expireDate, CancellationToken cancellationToken)
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

            AddNotification(new(nameof(existing.ExpireDate), message, ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange), notifications);
        }

        if (existing.State.Equals(ConfigurationState.Active) &&
            (NameHasChanges
            || StartDateHasChanges
            || ValueHasChanges))
        {
            var message = "it is not allowed to change name on active configuration";

            AddNotification(new(nameof(existing.StartDate), message, ConfigurationsErrorCodes.ErrorOnChangeName), notifications);
        }

        await DefaultValidationsAsync(existing.Id, name, value, description, startDate, expireDate, notifications, cancellationToken);

        return notifications;
    }

    private async Task DefaultValidationsAsync(Guid? id, string name, string value, string description, DateTime startDate, DateTime? expireDate,
        List<Notification> notifications, CancellationToken cancellationToken)
    {
        AddNotification(startDate.NotDefaultDateTime(), notifications);
        AddNotification(expireDate.NotDefaultDateTime(), notifications);
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 100), notifications);
        AddNotification(value.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(value.BetweenLength(1, 2500), notifications);
        AddNotification(description.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(description.BetweenLength(3, 1000), notifications);

        var listWithSameName = await _query.GetByNameAndStatesAsync(
            new DomainQueries.SearchConfigurationInput()
            {
                Name = name,
                States = [DomainQueries.ConfigurationState.Active, DomainQueries.ConfigurationState.Awaiting]
            }, cancellationToken);

        if (listWithSameName is not null && (id is null || (id is not null && listWithSameName.Exists(x => x.Id != id))))
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

    private static void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}
