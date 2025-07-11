namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationUpdated
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Value { get; init; } = "";
    public string Description { get; init; } = "";
    public DateTime StartDate { get; init; }
    public DateTime? ExpireDate { get; init; }
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public bool IsDeleted { get; init; }

    public static ConfigurationUpdated FromConfiguration(Configuration Configuration)
        => new ConfigurationUpdated() with
        {
            Id = Configuration.Id,
            Name = Configuration.Name,
            Value = Configuration.Value,
            Description = Configuration.Description,
            StartDate = Configuration.StartDate,
            ExpireDate = Configuration.ExpireDate,
            CreatedBy = Configuration.CreatedBy,
            CreatedAt = Configuration.CreatedAt,
            IsDeleted = Configuration.IsDeleted
        };
}
