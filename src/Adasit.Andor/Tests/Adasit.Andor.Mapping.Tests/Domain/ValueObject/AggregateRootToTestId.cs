using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace Adasit.Andor.Mapping.Tests.Domain.ValueObject;

internal readonly record struct AggregateRootToTestId(Guid Value) : IId<AggregateRootToTestId>
{
    public static AggregateRootToTestId New() => new AggregateRootToTestId(Guid.NewGuid());

    public static AggregateRootToTestId Load(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new AggregateRootToTestId(guid);
    }

    public static AggregateRootToTestId Load(Guid value) => new(value);

    public readonly override string ToString() => Value.ToString();

    public static implicit operator AggregateRootToTestId(Guid value) => new(value);

    public static implicit operator Guid(AggregateRootToTestId id) => id.Value;
}
