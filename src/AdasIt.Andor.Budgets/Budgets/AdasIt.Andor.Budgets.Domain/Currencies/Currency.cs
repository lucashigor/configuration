using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies;

public class Currency : Entity<CurrencyId>
{
    public string Name { get; private set; } = "";
    public string Iso { get; private set; } = "";
    public string Symbol { get; private set; } = "";
    private DomainResult SetValues(CurrencyId id,
        string name)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace());
        AddNotification(name.BetweenLength(3, 70));

        if (Notifications.Count > 1)
        {
            return Validate();
        }

        Name = name;

        var result = Validate();

        return result;
    }

    public static (DomainResult, Currency?) New(
        string name)
    {
        var entity = new Currency();

        var result = entity.SetValues(CurrencyId.New(), name);

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
