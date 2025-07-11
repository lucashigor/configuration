namespace AdasIt.Andor.Configurations.Tests.Domain.Helpers;
public record BaseConfiguration(string Name,
        string Value,
        string Description,
        DateTime StartDate,
        DateTime? ExpireDate);
