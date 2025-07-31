using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationDeleted : DomainEvent
{
    public static ConfigurationDeleted FromConfiguration(Configuration configuration)
        => new ConfigurationDeleted() with
        {
            Id = configuration.Id
        };
}
