using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Repository;

public interface IDefaultsProvider
{
    Task<IReadOnlyCollection<Category>> GetDefaultCategories(CancellationToken ct);
    Task<IReadOnlyCollection<PaymentMethod>> GetDefaultPaymentMethods(CancellationToken ct);
    Task<Currency> GetDefaultCurrency(CancellationToken ct);
}
