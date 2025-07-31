using AdasIt.Andor.Configurations.Application.Actors;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Domain.ValuesObjects;
using Akka.Actor;
using Akka.Hosting;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationCommandsService : IConfigurationCommandsService
{
    private readonly IActorRef _configActor;

    private TimeSpan Timeout =>
#if DEBUG
        TimeSpan.FromHours(2);
#else
    TimeSpan.FromSeconds(5);
#endif

    public ConfigurationCommandsService(ActorRegistry registry)
    {
        _configActor = registry.Get<ConfigurationManagerActor>();
    }

    public async Task<(DomainResult, ConfigurationOutput)> CreateConfigurationAsync(CreateConfiguration command, CancellationToken cancellationToken)
    {
        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, cancellationToken);

        return (result, new ConfigurationOutput()
        {
            Id = config.Id,
            Name = config.Name,
            Value = config.Value,
            Description = config.Description,
            CreatedAt = config.CreatedAt,
            CreatedBy = config.CreatedBy,
            ExpireDate = config.ExpireDate,
            StartDate = config.StartDate,
        });
    }

    public Task<DomainResult> DeleteConfigurationAsync(Guid configurationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<(DomainResult, ConfigurationOutput)> UpdateConfigurationAsync(UpdateConfiguration command, CancellationToken cancellationToken)
    {

        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, cancellationToken);

        return (result, new ConfigurationOutput()
        {
            Id = config.Id,
            Name = config.Name,
            Value = config.Value,
            Description = config.Description,
            CreatedAt = config.CreatedAt,
            CreatedBy = config.CreatedBy,
            ExpireDate = config.ExpireDate,
            StartDate = config.StartDate,
        });
    }
}
