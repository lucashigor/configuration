using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Repository;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Domain.SeedWork.Repositories;
using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Application.Actors;

public class ConfigurationActor : ReceiveActor, IWithUnboundedStash
{
    private readonly ConfigurationId _id;
    private readonly IConfigurationValidator _validator;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandsConfigurationRepository _commandRepository;
    private Configuration? _configuration;

    public IStash? Stash { get; set; }

    public ConfigurationActor(ConfigurationId id,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _validator = _serviceProvider.GetService<IConfigurationValidator>() ?? throw new InvalidOperationException(nameof(_validator));
        _commandRepository = _serviceProvider.GetService<ICommandsConfigurationRepository>() ?? throw new InvalidOperationException(nameof(ICommandRepository<Configuration, ConfigurationId>));

        _id = id;

        Become(Loading);
    }

    protected override void PreStart()
    {
        Self.Tell(new PreLoadConfiguration(_id));

        base.PreStart();
    }

    private void Loading()
    {
        ReceiveAsync<PreLoadConfiguration>(async cmd =>
        {
            var result = await _commandRepository.GetByIdAsync(_id, CancellationToken.None);

            if (result != null)
            {
                _configuration = result;

                Become(Ready);
                ProcessStash();
            }
        });

        ReceiveAsync<CreateConfiguration>(async cmd =>
        {
            var (result, config) = Configuration.New(
                _id,
                cmd.Name,
                cmd.Value,
                cmd.Description,
                cmd.StartDate,
                cmd.ExpireDate,
                Guid.NewGuid().ToString(),
                _validator);

            if (config != null && config.Events.Any())
            {
                await _commandRepository.PersistAsync(config, CancellationToken.None);
            }

            _configuration = config;

            Sender.Tell((result, config));
        });

        Receive<UpdateConfiguration>(msg => Stash!.Stash());
    }

    private void Ready()
    {
        ReceiveAsync<UpdateConfiguration>(async cmd =>
        {
            var result = _configuration!.Update(
                cmd.Name,
                cmd.Value,
                cmd.Description,
                cmd.StartDate,
                cmd.ExpireDate,
                _validator);

            if (_configuration.Events.Any())
            {
                await _commandRepository.PersistAsync(_configuration, cmd.CancellationToken);
            }

            Sender.Tell((result, _configuration));
        });
    }

    private void ProcessStash()
    {
        Stash!.UnstashAll();
    }

    public record PreLoadConfiguration(ConfigurationId Id);
}
