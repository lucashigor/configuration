using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.DomainQueries;

namespace AdasIt.Andor.Configurations.Application;

internal static class ConfigurationMapperExtensions
{
    public static ConfigurationOutput ToConfigurationOutput(this Configuration config)
        => new ConfigurationOutput()
        {
            Id = config.Id,
            Name = config.Name,
            Value = config.Value,
            Description = config.Description,
            CreatedAt = config.CreatedAt,
            CreatedBy = config.CreatedBy,
            ExpireDate = config.ExpireDate,
            StartDate = config.StartDate,
            State = new ConfigurationState(config.State.Key, config.State.Name)
        };
}