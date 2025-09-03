using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;

public readonly record struct PaymentMethodId(Guid Value) : IId<PaymentMethodId>
{
    public static PaymentMethodId New() => new PaymentMethodId(Guid.NewGuid());

    public static PaymentMethodId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new PaymentMethodId(guid);
    }

    public static PaymentMethodId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator PaymentMethodId(Guid value) => new(value);

    public static implicit operator Guid(PaymentMethodId id) => id.Value;
}
