using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;

namespace AdasIt.Andor.Budgets.InfrastructureCommands;

public class DefaultsProvider(ICategoryValidator categoryValidator,
        ICurrencyValidator currencyValidator,
        IPaymentMethodValidator paymentMethodValidator) : IDefaultsProvider
{

    public Task<IReadOnlyCollection<Category>> GetDefaultCategories(CancellationToken ct)
    {
        return Task.FromResult<IReadOnlyCollection<Category>>(new List<Category>()
        {
            Category.NewAsync(
                "Food",
                "Expenses for food and groceries",
                DateTime.UtcNow,
                null,
                MovementType.MoneySpending,
                categoryValidator,
                ct).Result.Item2!,
        });
    }

    public Task<Currency> GetDefaultCurrency(CancellationToken ct)
        => Task.FromResult(Currency.NewAsync("US Dollar", "USD", "$", currencyValidator, ct).Result.Item2!);

    public Task<IReadOnlyCollection<PaymentMethod>> GetDefaultPaymentMethods(CancellationToken ct)
        => Task.FromResult<IReadOnlyCollection<PaymentMethod>>(new List<PaymentMethod>()
        {
            PaymentMethod.NewAsync("Cash", "Cash payment method", DateTime.UtcNow, null, MovementType.MoneySpending, paymentMethodValidator, ct).Result.Item2!,
            PaymentMethod.NewAsync("Credit Card", "Credit Card payment method", DateTime.UtcNow, null, MovementType.MoneySpending, paymentMethodValidator, ct).Result.Item2!,
            PaymentMethod.NewAsync("Debit Card", "Debit Card payment method", DateTime.UtcNow, null, MovementType.MoneySpending, paymentMethodValidator, ct).Result.Item2!,
        });
}
