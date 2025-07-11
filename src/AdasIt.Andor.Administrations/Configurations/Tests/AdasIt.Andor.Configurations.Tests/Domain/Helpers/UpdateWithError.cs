using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Tests.Domain.Helpers;
public record UpdateWithError
{
    public ConfigurationState ConfigurationStatus { get; set; } = ConfigurationState.Undefined;
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public DomainErrorCode Error { get; set; } = CommonErrorCodes.Validation;
    public string FieldName { get; set; } = "";
    public string Because { get; set; } = "";
}
