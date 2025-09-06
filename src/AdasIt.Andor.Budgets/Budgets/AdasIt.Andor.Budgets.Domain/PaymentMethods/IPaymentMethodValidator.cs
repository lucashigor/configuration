using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public interface IPaymentMethodValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name, string description,
        DateTime startDate, DateTime? deactivationDate, MovementType type, CancellationToken cancellationToken);
}
