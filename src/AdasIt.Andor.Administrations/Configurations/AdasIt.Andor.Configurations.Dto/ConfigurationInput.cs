namespace AdasIt.Andor.Configurations.ApplicationDto;

public record ConfigurationInput(
    string Name,
    string Value,
    string Description,
    DateTime StartDate,
    DateTime? ExpireDate);