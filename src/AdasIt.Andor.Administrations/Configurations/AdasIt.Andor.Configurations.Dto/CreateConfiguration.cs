namespace AdasIt.Andor.Configurations.Dto;

public record CreateConfiguration(
    Guid id,
    string Name,
    string Value,
    string Description,
    DateTime StartDate,
    DateTime? ExpireDate,
    string CreatedBy)
{
    public CancellationToken CancellationToken { get; set; } = default;
}
