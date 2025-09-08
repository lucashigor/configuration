using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public class Currency : Entity<CurrencyId>
{
    public Name Name { get; private set; }
    public Iso Iso { get; private set; }
    public Symbol Symbol { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected Currency()
#pragma warning restore CS8618, CS9264
    {
        // For EF
    }

    private Currency(Name name, Iso iso, Symbol symbol)
    {
        Name = name;
        Iso = iso;
        Symbol = symbol;
    }

    public static async Task<(DomainResult, Currency?)> NewAsync(Name name,
        Iso iso,
        Symbol symbol,
        
        ICurrencyValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Currency(name, iso, symbol);
        
        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
