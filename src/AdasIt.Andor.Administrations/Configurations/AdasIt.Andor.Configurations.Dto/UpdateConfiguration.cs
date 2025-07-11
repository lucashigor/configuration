namespace AdasIt.Andor.Configurations.Dto;

public record UpdateConfiguration(
        Guid Id,
        string Name,
        string Value,
        string Description,
        DateTime StartDate,
        DateTime? ExpireDate,
        string CreatedBy);
