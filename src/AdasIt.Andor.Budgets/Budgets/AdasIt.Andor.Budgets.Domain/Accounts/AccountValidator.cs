using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public class AccountValidator : DefaultValidator<Account, AccountId>, IAccountValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        Account entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.Name.NotNull(), notifications);
        AddNotification(entity.Description.NotNull(), notifications);
    }
}
