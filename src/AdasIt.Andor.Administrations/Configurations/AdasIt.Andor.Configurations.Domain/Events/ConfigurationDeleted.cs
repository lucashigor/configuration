using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationDeleted : DomainEvent
{
    public Guid Id { get; init; }

    public static ConfigurationDeleted FromConfiguration(Configuration Configuration)
        => new ConfigurationDeleted() with
        {
            Id = Configuration.Id
        };
}
