using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;

public readonly record struct InviteId(Guid Value) : IId<InviteId>
{
    public static InviteId New() => new InviteId(Guid.NewGuid());

    public static InviteId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new InviteId(guid);
    }

    public static InviteId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator InviteId(Guid value) => new(value);

    public static implicit operator Guid(InviteId id) => id.Value;
}
