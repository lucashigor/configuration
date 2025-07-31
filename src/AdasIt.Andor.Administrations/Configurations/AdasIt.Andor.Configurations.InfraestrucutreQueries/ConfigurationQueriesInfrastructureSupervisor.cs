using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.DomainQueries;
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
    private const string _projectionName = "ConfigurationBasicProjection";

    public ConfigurationQueriesInfrastructureSupervisor(IEventPublisher eventPublisher,
        IServiceProvider serviceProvider)
    {
        _eventPublisher = eventPublisher;
            
        ReceiveAsync<ConfigurationCreated>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            _projectionName,
            serviceProvider,
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
            _projectionName,
            serviceProvider,
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
            _projectionName,
            serviceProvider,
            cmd.Id, cmd.EventId, db =>
        {
            var entity = db.Configuration.FirstOrDefault(x => x.Id == cmd.Id);
            if (entity == null)
                return Task.FromResult(false);

            db.Remove(entity);

            return Task.FromResult(true);
        }));


        ReceiveAsync<ConfigurationUpdated>(cmd => GenericHandleEventAsync.HandleEventAsync<ConfigurationContext>(
            _projectionName,
            serviceProvider,
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
