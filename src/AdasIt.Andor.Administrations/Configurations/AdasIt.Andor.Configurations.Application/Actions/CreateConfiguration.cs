using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.ValueObjects;

namespace AdasIt.Andor.Configurations.Application.Actions;

public record CreateConfiguration : ConfigurationAction
{
    private CreateConfiguration(ConfigurationId id, string name, string value, string description, DateTime startDate,
        DateTime? expireDate, CancellationToken cancellationToken) 
        : base(id, name, value, description, startDate, expireDate, cancellationToken)
    {
    }

    public static CreateConfiguration CreateInstance(ConfigurationInput input, CancellationToken cancellationToken)
    {
        return new CreateConfiguration(
            ConfigurationId.New(), 
            input.Name, 
            input.Value, 
            input.Description, 
            input.StartDate, 
            input.ExpireDate, 
            cancellationToken);
    }
}