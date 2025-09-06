using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;

public readonly record struct FinancialMovementId(Guid Value) : IId<FinancialMovementId>
{
    public static FinancialMovementId New() => new FinancialMovementId(Guid.NewGuid());

    public static FinancialMovementId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new FinancialMovementId(guid);
    }

    public static FinancialMovementId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator FinancialMovementId(Guid value) => new(value);

    public static implicit operator Guid(FinancialMovementId id) => id.Value;
}
