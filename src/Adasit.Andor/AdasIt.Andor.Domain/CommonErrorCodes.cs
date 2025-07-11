using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Domain;

public record CommonErrorCodes
{
    public static readonly DomainErrorCode General = DomainErrorCode.New(1_000);
    public static readonly DomainErrorCode Internal = DomainErrorCode.New(1_001);
    public static readonly DomainErrorCode Validation = DomainErrorCode.New(1_002);
    public static readonly DomainErrorCode InvalidYear = DomainErrorCode.New(1_003);
    public static readonly DomainErrorCode InvalidMonth = DomainErrorCode.New(1_004);
}
