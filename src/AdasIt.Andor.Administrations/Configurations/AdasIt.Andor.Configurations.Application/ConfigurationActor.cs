using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.Repository;
using AdasIt.Andor.Domain.ValuesObjects;
using Akka.Actor;
using Akka.Util.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Application
{
    public class ConfigurationActor : ReceiveActor
    {
        private readonly ConfigurationId _id;
        private readonly IActorRef _eventHandler;
        private readonly Queue<object> _stash = new();
        private readonly IConfigurationValidator _validator;
        private readonly IServiceProvider _serviceProvider;
        private Configuration? _configuration;

        public ConfigurationActor(ConfigurationId id, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _validator = _serviceProvider.GetService<IConfigurationValidator>() ?? throw new ArgumentNullException(nameof(IConfigurationValidator));

            _id = id;

            var childName = "persist" + _id.ToString();

            _eventHandler = Context.Child(childName);

            if (_eventHandler == ActorRefs.Nobody)
            {
                _eventHandler = Context.ActorOf(
                    Props.Create(() => new ConfigurationEventHandlerActor(_id)),
                    childName);
            }

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
                var result = await _eventHandler.Ask<Configuration?>(new LoadConfiguration(cmd.Id));

                if (result != null)
                {
                    _configuration = result;

                    Sender.Tell((DomainResult.Success(), result));

                    Become(Ready);
                    ProcessStash();
                }
            });

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

            Receive<UpdateConfiguration>(msg => _stash.Enqueue(msg));
        }

        private void Ready()
        {
            ReceiveAsync<GetConfiguration>(async cmd =>
            {
                Sender.Tell((DomainResult.Success(), _configuration));
            });

            ReceiveAsync<UpdateConfiguration>(async evt =>
            {
                Console.WriteLine($"[{_id}] Configuração alterada: {evt.Value}");
            });
        }

        private void ProcessStash()
        {
            while (_stash.TryDequeue(out var msg))
            {
                Self.Tell(msg);
            }
        }

        public record PreLoadConfiguration(ConfigurationId Id);
    }
}
