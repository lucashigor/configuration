using System.Net.Mail;

namespace AdasIt.Andor.Domain.ValuesObjects;

public record Email : StringValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 254;

    private Email(string value) : base(new MailAddress(value).Address, MinLength, MaxLength, nameof(Email))
    {
    }

    public static implicit operator Email(string value) => new(value);

    public static implicit operator string(Email symbol) => symbol.Value;
}
