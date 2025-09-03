using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public interface IAccountValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name, string description, Currency? currency,
        AccountStatus status, CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(Account current, string name, string description,
        Currency currency, CancellationToken cancellationToken);
}
