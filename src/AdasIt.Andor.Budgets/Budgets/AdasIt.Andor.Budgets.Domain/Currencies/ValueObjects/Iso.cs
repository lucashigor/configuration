using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;

public record Iso : StringValueObject
{
    private const int MinLength = 2;
    public const int MaxLength = 3;

    private Iso(string value) : base(value, MinLength, MaxLength, nameof(Iso)) { }

    public static Iso Load(string value) => new(value);

    public override string ToString() => Value;

    public static implicit operator Iso(string value) => new(value);

    public static implicit operator string(Iso symbol) => symbol.Value;
}