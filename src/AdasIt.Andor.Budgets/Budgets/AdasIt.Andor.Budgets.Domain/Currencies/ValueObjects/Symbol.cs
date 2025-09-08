using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;

public record Symbol : StringValueObject
{
    private const int MinLength = 2;
    public const int MaxLength = 3;

    private Symbol(string value) : base(value, MinLength, MaxLength, nameof(Symbol)) { }

    public static Symbol Load(string value) => new(value);

    public override string ToString() => Value;

    public static implicit operator Symbol(string value) => new(value);

    public static implicit operator string(Symbol symbol) => symbol.Value;
}