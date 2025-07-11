using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.SeedWork.Repositories.CommandRepository;

namespace AdasIt.Andor.Configurations.Domain.Repository;

public interface ICommandsConfigurationRepository : ICommandRepository<Configuration, ConfigurationId>
{
}