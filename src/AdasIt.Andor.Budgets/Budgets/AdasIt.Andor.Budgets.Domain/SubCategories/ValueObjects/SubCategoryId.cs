using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;

public readonly record struct SubCategoryId(Guid Value) : IId<SubCategoryId>
{
    public static SubCategoryId New() => new SubCategoryId(Guid.NewGuid());

    public static SubCategoryId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new SubCategoryId(guid);
    }

    public static SubCategoryId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator SubCategoryId(Guid value) => new(value);

    public static implicit operator Guid(SubCategoryId id) => id.Value;
}
