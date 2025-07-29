using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using AdasIt.Andor.Infrastructure;
using AdasIt.Andor.InfrastructureQueries;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.InfrastructureQueries;

public class ConfigurationQueriesInfrastructureSupervisor : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IServiceProvider _serviceProvider;

    private readonly string ProjectionName = "ConfigurationBasicProjection";

    public ConfigurationQueriesInfrastructureSupervisor(IEventPublisher eventPublisher,
        IServiceProvider serviceProvider)
    {
        _eventPublisher = eventPublisher;
        _serviceProvider = serviceProvider;

        ReceiveAsync<GetConfiguration>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var entity = await db.Configuration
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(cmd.Id));

            if (entity == null)
            {
                Sender.Tell(null);

                return;
            }

            var state = Domain.Configuration.GetStatus(false, entity.StartDate, entity.ExpireDate);

            entity.State = new DomainQueries.ConfigurationState(state.Key, state.Name);

            Sender.Tell(entity);
        });

        ReceiveAsync<ConfigurationCreated>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            ProjectionName,
            _serviceProvider,
            cmd.Id, cmd.EventId, db =>
        {
            var entity = new ConfigurationOutput
            {
                Id = cmd.Id,
                Name = cmd.Name,
                Value = cmd.Value,
                Description = cmd.Description,
                StartDate = cmd.StartDate,
                ExpireDate = cmd.ExpireDate,
                CreatedBy = cmd.CreatedBy,
                CreatedAt = cmd.CreatedAt
            };

            db.Upsert(entity);

            return Task.FromResult(true);
        }));

        ReceiveAsync<ConfigurationDeactivated>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            ProjectionName,
            _serviceProvider,
            cmd.Id, cmd.EventId, db =>
        {
            var entity = db.Configuration.FirstOrDefault(x => x.Id == cmd.Id);
            if (entity == null)
                return Task.FromResult(false);

            entity.ExpireDate = cmd.ExpireDate;
            db.Upsert(entity);

            return Task.FromResult(true);
        }));

        ReceiveAsync<ConfigurationDeleted>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            ProjectionName,
            _serviceProvider,
            cmd.Id, cmd.EventId, db =>
        {
            var entity = db.Configuration.FirstOrDefault(x => x.Id == cmd.Id);
            if (entity == null)
                return Task.FromResult(false);

            db.Remove(entity);

            return Task.FromResult(true);
        }));


        ReceiveAsync<ConfigurationUpdated>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            ProjectionName,
            _serviceProvider,
            cmd.Id, cmd.EventId, db =>
        {
            var entity = db.Configuration.FirstOrDefault(x => x.Id == cmd.Id);
            if (entity == null)
                return Task.FromResult(false);

            entity.Name = cmd.Name;
            entity.Value = cmd.Value;
            entity.Description = cmd.Description;
            entity.StartDate = cmd.StartDate;
            entity.ExpireDate = cmd.ExpireDate;

            db.Upsert(entity);

            return Task.FromResult(true);
        }));
    }

    protected override void PreStart()
    {
        _eventPublisher.SubscribeAsync(Self.Tell, CancellationToken.None).GetAwaiter().GetResult();
    }
}
