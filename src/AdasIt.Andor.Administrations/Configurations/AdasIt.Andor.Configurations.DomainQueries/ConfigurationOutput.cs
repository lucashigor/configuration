using AdasIt.Andor.DomainQueries;

namespace AdasIt.Andor.Configurations.DomainQueries;

public record ConfigurationOutput : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ConfigurationState State { get; set; } = ConfigurationState.Undefined;
}
