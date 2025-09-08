using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.FinancialMovements;

public class FinancialMovementValidator : DefaultValidator<FinancialMovement, FinancialMovementId>, IFinancialMovementValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        FinancialMovement entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);

        AddNotification(entity.Value.NotNull(), notifications);
    }
}
