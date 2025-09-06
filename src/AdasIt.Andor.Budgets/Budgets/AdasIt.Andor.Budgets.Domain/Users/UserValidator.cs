using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Users;

public class UserValidator : IUserValidator
{
    public async Task<List<Notification>> ValidateCreationAsync(string name,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, notifications, cancellationToken);

        return notifications;
    }

    public async Task<List<Notification>> ValidateUpdateAsync(string name,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, notifications, cancellationToken);

        return notifications;
    }

    private Task DefaultValidationsAsync(
        string name,
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
