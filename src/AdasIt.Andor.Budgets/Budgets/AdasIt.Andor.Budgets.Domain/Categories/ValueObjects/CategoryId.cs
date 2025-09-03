using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;

public readonly record struct CategoryId(Guid Value) : IId<CategoryId>
{
    public static CategoryId New() => new CategoryId(Guid.NewGuid());

    public static CategoryId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new CategoryId(guid);
    }

    public static CategoryId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator CategoryId(Guid value) => new(value);

    public static implicit operator Guid(CategoryId id) => id.Value;
}