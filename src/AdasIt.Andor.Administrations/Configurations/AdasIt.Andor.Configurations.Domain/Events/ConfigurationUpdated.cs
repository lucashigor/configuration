﻿using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Configurations.Domain.Events;

public record ConfigurationUpdated : DomainEvent
{
    public string Name { get; private init; } = "";
    public string Value { get; private init; } = "";
    public string Description { get; private init; } = "";
    public DateTime StartDate { get; private init; }
    public DateTime? ExpireDate { get; private init; }

    public static ConfigurationUpdated FromConfiguration(Configuration configuration)
        => new ConfigurationUpdated() with
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Value = configuration.Value,
            Description = configuration.Description,
            StartDate = configuration.StartDate,
            ExpireDate = configuration.ExpireDate
        };
}
