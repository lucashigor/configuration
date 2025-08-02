using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain.Errors;

public record ConfigurationsErrorCodes
{
    public static readonly DomainErrorCode ErrorOnDeleteConfiguration = DomainErrorCode.New(2_001);
    public static readonly DomainErrorCode OnlyDescriptionAllowedToChange = DomainErrorCode.New(2_002);
    public static readonly DomainErrorCode ErrorOnChangeName = DomainErrorCode.New(2_003);
    public static readonly DomainErrorCode SetExpireDateToToday = DomainErrorCode.New(2_004);
    public static readonly DomainErrorCode ThereWillCurrentConfigurationStartDate = DomainErrorCode.New(2_005);
    public static readonly DomainErrorCode ThereWillCurrentConfigurationEndDate = DomainErrorCode.New(2_006);
}
