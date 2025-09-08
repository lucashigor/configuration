namespace AdasIt.Andor.Domain.ValuesObjects;

public record Name : StringValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 70;

    private Name(string value) : base(value, MinLength, MaxLength, nameof(Name)) { }

    public static implicit operator Name(string value) => new(value);

    public static implicit operator string(Name symbol) => symbol.Value;
}