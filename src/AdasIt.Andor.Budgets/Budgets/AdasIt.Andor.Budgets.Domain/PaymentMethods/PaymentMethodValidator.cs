using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public class PaymentMethodValidator : DefaultValidator<PaymentMethod, PaymentMethodId>, IPaymentMethodValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        PaymentMethod entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.Name.Value.NotNullOrEmptyOrWhiteSpace(), notifications);
        
        AddNotification(entity.Description.Value.NotNullOrEmptyOrWhiteSpace(), notifications);
        
        AddNotification(entity.StartDate.NotNull(), notifications);
        AddNotification(entity.StartDate.NotDefaultDateTime(), notifications);
        
        AddNotification(entity.DeactivationDate.NotDefaultDateTime(), notifications);
    }
}
