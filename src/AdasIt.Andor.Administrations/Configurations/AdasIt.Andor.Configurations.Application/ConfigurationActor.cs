using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.Repository;
using Akka.Actor;
using Akka.Util.Internal;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Application
{
    public class ConfigurationActor : ReceiveActor
    {
        private readonly ConfigurationId _id;
        private readonly IConfigurationValidator _validator;
        private readonly IActorRef _eventHandler;

        public ConfigurationActor(ConfigurationId id, IServiceProvider _serviceProvider)
        {
            using var scope = _serviceProvider.CreateScope();
            var validator = _serviceProvider.GetService<IConfigurationValidator>() ?? throw new ArgumentNullException(nameof(IConfigurationValidator));
            var mapper = _serviceProvider.GetService<IMapper>() ?? throw new ArgumentNullException(nameof(IMapper));

            _id = id;
            _validator = validator;

            _eventHandler = Context.ActorOf(Props.Create(() =>
            new ConfigurationEventHandlerActor(_serviceProvider, mapper)), "event-handler");

            Context.System.EventStream.Subscribe(Self, typeof(CreateConfiguration));

            Receive<CreateConfiguration>(cmd =>
            {
                if (cmd.CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var (result, config) = Configuration.New(
                    _id,
                    cmd.Name,
                    cmd.Value,
                    cmd.Description,
                    cmd.StartDate,
                    cmd.ExpireDate,
                    cmd.CreatedBy,
                    _validator);

                if (!cmd.CancellationToken.IsCancellationRequested && config != null && config.Events.Any())
                {
                    config.Events.ForEach(e => _eventHandler.Tell(e));
                }

                Sender.Tell((result, config));
                return;
            });
        }
    }
}
