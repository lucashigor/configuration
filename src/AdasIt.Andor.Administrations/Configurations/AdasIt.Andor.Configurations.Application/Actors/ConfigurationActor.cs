using AdasIt.Andor.Configurations.Application.Actions;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Repository;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Application.Actors;

public class ConfigurationActor : ReceiveActor, IWithUnboundedStash
{
    private readonly ConfigurationId _id;
    private Configuration? _configuration;
    private readonly IServiceProvider _serviceProvider;

    public IStash? Stash { get; set; }

    public ConfigurationActor(ConfigurationId id,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

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
            using var scope = _serviceProvider.CreateScope();
            var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandsConfigurationRepository>();

            var result = await commandRepository.GetByIdAsync(_id, CancellationToken.None);

            if (result == null) return;

            _configuration = result;

            Become(Ready);
            ProcessStash();
        });

        ReceiveAsync<CreateConfiguration>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IConfigurationValidator>();
            var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandsConfigurationRepository>();

            var (result, config) = await Configuration.NewAsync(
                _id,
                cmd.Name,
                cmd.Value,
                cmd.Description,
                cmd.StartDate,
                cmd.ExpireDate,
                Guid.NewGuid().ToString(),
                validator,
                cmd.CancellationToken);

            if (config != null && config.Events.Count != 0)
            {
                await commandRepository.PersistAsync(config, CancellationToken.None);
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
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IConfigurationValidator>();
            var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandsConfigurationRepository>();

            var result = _configuration!.UpdateAsync(
                cmd.Name,
                cmd.Value,
                cmd.Description,
                cmd.StartDate,
                cmd.ExpireDate,
                validator,
                cmd.CancellationToken);

            if (_configuration.Events.Count != 0)
            {
                await commandRepository.PersistAsync(_configuration, cmd.CancellationToken);
            }

            Sender.Tell((result, _configuration));
        });
    }

    private void ProcessStash()
    {
        Stash!.UnstashAll();
    }

    private record PreLoadConfiguration(ConfigurationId Id);
}
