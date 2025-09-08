using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.SeedWork.Repositories;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods.Repository;

public interface ICommandsPaymentMethodRepository :
    ICommandRepository<PaymentMethod, PaymentMethodId>
{
    Task<IEnumerable<PaymentMethod>> GetDefaultPaymentMethods(CancellationToken cancellationToken);
}