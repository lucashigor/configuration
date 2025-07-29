using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationDeactivated : DomainEvent
{
    public Guid Id { get; init; }
    public DateTime? ExpireDate { get; init; }

    public static ConfigurationDeactivated FromConfiguration(Configuration Configuration)
        => new ConfigurationDeactivated() with
        {
            Id = Configuration.Id,
            ExpireDate = Configuration.ExpireDate
        };
}
