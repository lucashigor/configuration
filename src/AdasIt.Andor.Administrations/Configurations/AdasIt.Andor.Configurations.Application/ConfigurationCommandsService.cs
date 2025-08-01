using AdasIt.Andor.ApplicationDto;
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

    public async Task<ApplicationResult<ConfigurationOutput>> CreateConfigurationAsync(ConfigurationInput input,
        CancellationToken cancellationToken)
    {
        var command = CreateConfiguration.CreateInstance(input, cancellationToken);

        return await Handler(command, cancellationToken);
    }

    public async Task<ApplicationResult<ConfigurationOutput>> UpdateConfigurationAsync(ConfigurationId id, ConfigurationInput input,
        CancellationToken cancellationToken)
    {
        var command = UpdateConfiguration.CreateInstance(id, input, cancellationToken);
        return await Handler(command, cancellationToken);
    }

    public Task<ApplicationResult<object>> DeleteConfigurationAsync(ConfigurationId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    private async Task<ApplicationResult<ConfigurationOutput>> Handler(object command, CancellationToken cancellationToken)
    {
        var response = ApplicationResult<ConfigurationOutput>.Success();

        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command,
            cancellationToken);

        if (result.IsFailure)
        {
            await HandleConfigurationResult.HandleResultConfiguration(result, response);
            return response;
        }

        response.SetData(config.ToConfigurationOutput());

        return response;
    }
}
