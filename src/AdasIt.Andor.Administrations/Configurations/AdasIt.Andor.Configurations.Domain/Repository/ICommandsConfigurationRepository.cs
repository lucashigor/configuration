using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.SeedWork.Repositories;

namespace AdasIt.Andor.Configurations.Domain.Repository;

public interface ICommandsConfigurationRepository :
    ICommandRepository<Configuration, ConfigurationId>
{
}