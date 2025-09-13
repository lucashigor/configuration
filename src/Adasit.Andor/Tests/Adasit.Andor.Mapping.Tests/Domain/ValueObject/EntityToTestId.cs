using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace Adasit.Andor.Mapping.Tests.Domain.ValueObject;

internal readonly record struct EntityToTestId(Guid Value) : IId<EntityToTestId>
{
    public static EntityToTestId New() => new EntityToTestId(Guid.NewGuid());

    public static EntityToTestId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new EntityToTestId(guid);
    }

    public static EntityToTestId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator EntityToTestId(Guid value) => new(value);

    public static implicit operator Guid(EntityToTestId id) => id.Value;
}
