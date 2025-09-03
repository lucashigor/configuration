using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Users.ValueObjects;

public readonly record struct LanguageId(Guid Value) : IId<LanguageId>
{
    public static LanguageId New() => new LanguageId(Guid.NewGuid());

    public static LanguageId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new LanguageId(guid);
    }

    public static LanguageId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator LanguageId(Guid value) => new(value);

    public static implicit operator Guid(LanguageId id) => id.Value;
}
