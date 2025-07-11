using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public class ConfigurationValidator : IConfigurationValidator
{
    public List<Notification> ValidateCreation(string name, string value, string description, DateTime startDate, DateTime? expireDate)
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

        DefaultValidations(name, value, description, startDate, expireDate, notifications);

        return notifications;
    }

    public List<Notification> ValidateUpdate(Configuration existing, string name, string value, string description, DateTime startDate, DateTime? expireDate)
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

        DefaultValidations(name, value, description, startDate, expireDate, notifications);

        return notifications;
    }

    private void DefaultValidations(string name, string value, string description, DateTime startDate, DateTime? expireDate,
        List<Notification> notifications)
    {
        AddNotification(startDate.NotDefaultDateTime(), notifications);
        AddNotification(expireDate.NotDefaultDateTime(), notifications);
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 100), notifications);
        AddNotification(value.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(value.BetweenLength(1, 2500), notifications);
        AddNotification(description.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(description.BetweenLength(3, 1000), notifications);
    }

    private void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}
