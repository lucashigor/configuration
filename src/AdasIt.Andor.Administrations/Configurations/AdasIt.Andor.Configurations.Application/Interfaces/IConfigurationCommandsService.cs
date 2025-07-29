using AdasIt.Andor.Configurations.Dto;

namespace AdasIt.Andor.Configurations.Application.Interfaces;

public interface IConfigurationCommandsService
{
    Task CreateConfigurationAsync(CreateConfiguration command, CancellationToken cancellationToken);
    Task UpdateConfigurationAsync(UpdateConfiguration command, CancellationToken cancellationToken);
    Task DeleteConfigurationAsync(Guid configurationId, CancellationToken cancellationToken);
}
