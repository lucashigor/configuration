using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Users.ValueObjects;

public readonly record struct UserId(Guid Value) : IId<UserId>
{
    public static UserId New() => new UserId(Guid.NewGuid());

    public static UserId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new UserId(guid);
    }

    public static UserId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator UserId(Guid value) => new(value);

    public static implicit operator Guid(UserId id) => id.Value;
}
