using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;

public readonly record struct CurrencyId(Guid Value) : IId<CurrencyId>
{
    public static CurrencyId New() => new CurrencyId(Guid.NewGuid());

    public static CurrencyId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new CurrencyId(guid);
    }

    public static CurrencyId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator CurrencyId(Guid value) => new(value);

    public static implicit operator Guid(CurrencyId id) => id.Value;
}
