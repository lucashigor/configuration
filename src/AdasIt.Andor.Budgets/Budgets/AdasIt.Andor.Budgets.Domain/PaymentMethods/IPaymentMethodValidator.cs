using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public interface IPaymentMethodValidator : IDefaultValidator<PaymentMethod, PaymentMethodId>
{
}
