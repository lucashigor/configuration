using AdasIt.Andor.Configurations.Application.Actions;
using AdasIt.Andor.Configurations.Application.Actors;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Domain.ValuesObjects;
using Akka.Actor;
using Akka.Hosting;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationCommandsService(ActorRegistry registry) : IConfigurationCommandsService
{
    private readonly IActorRef _configActor = registry.Get<ConfigurationManagerActor>();

    public async Task<(DomainResult, ConfigurationOutput)> CreateConfigurationAsync(ConfigurationInput input, 
        CancellationToken cancellationToken)
    {
        var command = CreateConfiguration.CreateInstance(input, cancellationToken);
        
        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, 
            cancellationToken);

        return (result, config.ToConfigurationOutput());
    }

    public Task<DomainResult> DeleteConfigurationAsync(Guid configurationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<(DomainResult, ConfigurationOutput)> UpdateConfigurationAsync(ConfigurationId id, ConfigurationInput input, 
        CancellationToken cancellationToken)
    {
        var command = UpdateConfiguration.CreateInstance(id, input, cancellationToken);
        
        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, 
            cancellationToken);

        return (result, config.ToConfigurationOutput());
    }

    public Task<DomainResult> DeleteConfigurationAsync(ConfigurationId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
