using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;

public readonly record struct AccountId(Guid Value) : IId<AccountId>
{
    public static AccountId New() => new AccountId(Guid.NewGuid());

    public static AccountId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new AccountId(guid);
    }

    public static AccountId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator AccountId(Guid value) => new(value);

    public static implicit operator Guid(AccountId id) => id.Value;
}
