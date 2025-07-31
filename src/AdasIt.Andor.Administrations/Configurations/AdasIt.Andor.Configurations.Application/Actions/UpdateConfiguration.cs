using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using Akka.Streams.Util;

namespace AdasIt.Andor.Configurations.Application.Actions;

public record UpdateConfiguration : ConfigurationAction
{
    private UpdateConfiguration(ConfigurationId id, string name, string value, string description, DateTime startDate,
        DateTime? expireDate, CancellationToken cancellationToken) 
        : base(id, name, value, description, startDate, expireDate, cancellationToken)
    {
    }

    public static UpdateConfiguration CreateInstance(Guid id, ConfigurationInput input, 
        CancellationToken cancellationToken)
    {
        return new UpdateConfiguration(
            id, 
            input.Name, 
            input.Value, 
            input.Description, 
            input.StartDate, 
            input.ExpireDate, 
            cancellationToken);
    }
}