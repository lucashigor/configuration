using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Application.Interfaces;

public interface IConfigurationCommandsService
{
    Task<(DomainResult, ConfigurationOutput)> CreateConfigurationAsync(ConfigurationInput command, 
        CancellationToken cancellationToken);
    Task<(DomainResult, ConfigurationOutput)> UpdateConfigurationAsync(ConfigurationId id, ConfigurationInput command, 
        CancellationToken cancellationToken);
    Task<DomainResult> DeleteConfigurationAsync(ConfigurationId id, CancellationToken cancellationToken);
}
