using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.Infrastructure.Config;
using AdasIt.Andor.Configurations.Infrastructure.Repositories;
using Akka.Actor;
using Akka.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Repository;

/*
public class ConfigurationEventHandlerActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public ConfigurationEventHandlerActor(IServiceProvider serviceProvider,
        IMapper mapper)
    {

        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        Context.System.EventStream.Subscribe(Self, typeof(LoadConfiguration));
        Context.System.EventStream.Subscribe(Self, typeof(ConfigurationCreated));
        Context.System.EventStream.Subscribe(Self, typeof(UpdateConfiguration));

        Receive<LoadConfiguration>(evt =>
        {
            using var scope = serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var result = _context.Configuration.AsNoTracking().FirstOrDefault(x => x.Id == evt.ConfigId);

            Sender.Tell(result);
        });

        Receive<ConfigurationCreated>(evt =>
        {
            using var scope = serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
            var config = _mapper.Map<ConfigurationDto>(evt);

            _context.Configuration.Add(config);

            _context.SaveChanges();
        });

        Receive<UpdateConfiguration>(evt =>
        {
            using var scope = serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
            var result = _context.Configuration.AsNoTracking().FirstOrDefault(x => x.Id == evt.Id);

            result = new ConfigurationDto();

            _context.SaveChanges();
        });
    }
}
*/

public class ConfigurationEventHandlerActor : ReceivePersistentActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public override string PersistenceId => "configuration-handler";

    public ConfigurationEventHandlerActor(IServiceProvider serviceProvider, IMapper mapper)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        // Command Handlers
        Command<LoadConfiguration>(HandleLoadConfiguration);
        Command<ConfigurationCreated>(HandleConfigurationCreated);
        Command<UpdateConfiguration>(HandleUpdateConfiguration);

        // Event Handlers (used during recovery)
        Recover<ConfigurationCreated>(_ => { /* could rebuild state */ });
        Recover<UpdateConfiguration>(_ => { /* could rebuild state */ });
    }

    private void HandleLoadConfiguration(LoadConfiguration evt)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

        var result = context.Configuration.AsNoTracking().FirstOrDefault(x => x.Id == evt.ConfigId);

        Sender.Tell(result);
    }

    private void HandleConfigurationCreated(ConfigurationCreated evt)
    {
        Persist(evt, e =>
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
            var config = _mapper.Map<ConfigurationDto>(e);

            context.Configuration.Add(config);
            context.SaveChanges();
        });
    }

    private void HandleUpdateConfiguration(UpdateConfiguration evt)
    {
        Persist(evt, e =>
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var existing = context.Configuration.FirstOrDefault(x => x.Id == e.Id);
            if (existing != null)
            {
                _mapper.Map(e, existing);
                context.SaveChanges();
            }
        });
    }
}
