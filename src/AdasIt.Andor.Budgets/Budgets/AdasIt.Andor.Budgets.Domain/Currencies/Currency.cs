using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public class Currency : Entity<CurrencyId>
{
    public string Name { get; private set; } = "";
    public string Iso { get; private set; } = "";
    public string Symbol { get; private set; } = "";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Currency()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // For EF
    }

    private Currency(string name,
        string iso,
        string symbol)
    {
        Name = name;
        Iso = iso;
        Symbol = symbol;
    }

    public static async Task<(DomainResult, Currency?)> NewAsync(string name,
        string iso,
        string symbol,
        ICurrencyValidator currencyValidator,
        CancellationToken cancellationToken)
    {
        var entity = new Currency(name, iso, symbol);

        var notifications = await currencyValidator.ValidateCreationAsync(entity.Name,
            entity.Iso, entity.Symbol, cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
