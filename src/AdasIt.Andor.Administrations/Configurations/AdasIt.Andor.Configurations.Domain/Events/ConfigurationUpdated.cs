using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationUpdated : DomainEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Value { get; init; } = "";
    public string Description { get; init; } = "";
    public DateTime StartDate { get; init; }
    public DateTime? ExpireDate { get; init; }

    public static ConfigurationUpdated FromConfiguration(Configuration Configuration)
        => new ConfigurationUpdated() with
        {
            Id = Configuration.Id,
            Name = Configuration.Name,
            Value = Configuration.Value,
            Description = Configuration.Description,
            StartDate = Configuration.StartDate,
            ExpireDate = Configuration.ExpireDate
        };
}
