using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public class CurrencyValidator : ICurrencyValidator
{
    public async Task<List<Notification>> ValidateCreationAsync(string name,
        string iso,
        string symbol,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, iso, symbol, notifications, cancellationToken);

        return notifications;
    }

    public async Task<List<Notification>> ValidateUpdateAsync(string name,
        string iso,
        string symbol, CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, iso, symbol, notifications, cancellationToken);

        return notifications;
    }

    private Task DefaultValidationsAsync(
        string name,
        string iso,
        string symbol,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 70), notifications);

        AddNotification(iso.BetweenLength(2, 3), notifications);
        AddNotification(iso.NotNullOrEmptyOrWhiteSpace(), notifications);

        AddNotification(symbol.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(symbol.BetweenLength(2, 10), notifications);

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
