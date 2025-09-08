using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public class CurrencyValidator : DefaultValidator<Currency, CurrencyId>, ICurrencyValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        Currency entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);

        AddNotification(entity.Name.NotNull(), notifications);
        AddNotification(entity.Iso.NotNull(), notifications);
        AddNotification(entity.Symbol.NotNull(), notifications);
    }
}
