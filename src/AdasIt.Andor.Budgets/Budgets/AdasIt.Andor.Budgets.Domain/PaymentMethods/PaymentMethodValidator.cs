using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public class PaymentMethodValidator : IPaymentMethodValidator
{
    public async Task<List<Notification>> ValidateCreationAsync(string name, string description,
        DateTime startDate, DateTime? deactivationDate, MovementType type,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, description,
        startDate, deactivationDate, type, notifications, cancellationToken);

        return notifications;
    }

    private Task DefaultValidationsAsync(string name, string description,
        DateTime startDate, DateTime? deactivationDate, MovementType type,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 70), notifications);

        return Task.CompletedTask;
    }

    private static void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}
