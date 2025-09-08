using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using NSubstitute;

namespace AdasIt.Andor.Budgets.Tests.Domain;

internal static class PaymentMethodFixture
{
    public static PaymentMethod GetValidDepositPaymentMethod()
        => GetValidPaymentMethod(MovementType.MoneyDeposit);
    
    public static PaymentMethod GetValidPaymentMethod(MovementType movementType)
    {
        var paymentMethod = GeneralFixture.CreateInstanceAndSetProperties<PaymentMethod>(
            new Dictionary<string, object>()
            {
                { nameof(PaymentMethod.Id), PaymentMethodId.New() },
                { nameof(PaymentMethod.Name), GeneralFixture.GetValidName() },
                { nameof(PaymentMethod.Description), GeneralFixture.GetValidDescription() },
                { nameof(PaymentMethod.StartDate), DateTime.Now },
                { nameof(PaymentMethod.DeactivationDate), null },
                { nameof(PaymentMethod.Type), movementType },
            });
        
        return paymentMethod;
    }
}
