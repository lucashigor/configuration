using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public interface ICurrencyValidator : IDefaultValidator<Currency, CurrencyId>
{
}
