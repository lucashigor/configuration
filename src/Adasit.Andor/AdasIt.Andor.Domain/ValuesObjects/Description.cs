namespace AdasIt.Andor.Domain.ValuesObjects;

public record Description : StringValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 250;

    private Description(string value) : base(value, MinLength, MaxLength, nameof(Description)) { }

    public static implicit operator Description(string value) => new(value);

    public static implicit operator string(Description symbol) => symbol.Value;
}