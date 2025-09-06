using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public interface ICurrencyValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name,
        string iso,
        string symbol,
        CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(string name,
        string iso,
        string symbol,
        CancellationToken cancellationToken);
}
