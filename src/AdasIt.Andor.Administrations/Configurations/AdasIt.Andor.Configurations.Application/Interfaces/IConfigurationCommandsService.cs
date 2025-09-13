using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;

namespace AdasIt.Andor.Configurations.Application.Interfaces;

public interface IConfigurationCommandsService
{
    Task<ApplicationResult<ConfigurationOutput>> CreateConfigurationAsync(ConfigurationInput command,
        CancellationToken cancellationToken);
    Task<ApplicationResult<ConfigurationOutput>> UpdateConfigurationAsync(ConfigurationId id, ConfigurationInput command,
        CancellationToken cancellationToken);
    Task<ApplicationResult<object>> DeleteConfigurationAsync(ConfigurationId id, CancellationToken cancellationToken);
}
