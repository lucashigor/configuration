using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Application.Interfaces;

public interface IConfigurationCommandsService
{
    Task<(DomainResult, ConfigurationOutput)> CreateConfigurationAsync(CreateConfiguration command, CancellationToken cancellationToken);
    Task<(DomainResult, ConfigurationOutput)> UpdateConfigurationAsync(UpdateConfiguration command, CancellationToken cancellationToken);
    Task<DomainResult> DeleteConfigurationAsync(Guid configurationId, CancellationToken cancellationToken);
}
