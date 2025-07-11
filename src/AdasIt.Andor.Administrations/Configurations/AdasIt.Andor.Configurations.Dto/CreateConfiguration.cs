namespace AdasIt.Andor.Configurations.Dto;

public record CreateConfiguration(
    string Name,
    string Value,
    string Description,
    DateTime StartDate,
    DateTime? ExpireDate,
    string CreatedBy)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public CancellationToken CancellationToken { get; set; } = default;
}
