using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using AdasIt.Andor.Domain.Events;
using AdasIt.Andor.InfrastructureQueries;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.InfrastructureQueries;

public class ConfigurationQueriesEventHandler : ReceiveActor
{
    private readonly ConfigurationId _id;
    private readonly IServiceProvider _serviceProvider;
    public ConfigurationQueriesEventHandler(ConfigurationId id,
        IServiceProvider serviceProvider)
    {
        _id = id;
        _serviceProvider = serviceProvider;

        Receive<GetConfiguration>(cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var entity = db.Configuration
                .AsNoTracking()
                .FirstOrDefault(x => x.Id.Equals(cmd.Id));


            if (entity == null)
            {
                Sender.Tell(null);

                return;
            }

            var state = Domain.Configuration.GetStatus(false, entity.StartDate, entity.ExpireDate);

            entity.State = new DomainQueries.ConfigurationState(state.Key, state.Name);

            Sender.Tell(entity);
        });

        ReceiveAsync<ConfigurationCreated>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var processedEvent = db.ProcessedEvents.Find(cmd.Id, cmd.EventId, "ConfigurationBasicProjection");

            if (processedEvent != null)
                return;
            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();

                try
                {
                    var entity = new ConfigurationOutput()
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
                    db.Upsert(new ProcessedEvents(cmd.Id, "ConfigurationBasicProjection", cmd.EventId, DateTime.UtcNow));

                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        ReceiveAsync<ConfigurationDeactivated>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var processedEvent = db.ProcessedEvents.Find(cmd.Id, cmd.EventId, "ConfigurationBasicProjection");

            if (processedEvent != null)
                return;
            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();

                try
                {
                    var entity = db.Configuration.FirstOrDefault(x => x.Id.Equals(cmd.Id));

                    if (entity == null)
                    {
                        await transaction.RollbackAsync();
                        return;
                    }

                    entity.ExpireDate = cmd.ExpireDate;

                    db.Upsert(entity);
                    db.Upsert(new ProcessedEvents(cmd.Id, "ConfigurationBasicProjection", cmd.EventId, DateTime.UtcNow));

                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        ReceiveAsync<ConfigurationDeleted>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var processedEvent = db.ProcessedEvents.Find(cmd.Id, cmd.EventId, "ConfigurationBasicProjection");

            if (processedEvent != null)
                return;
            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();

                try
                {
                    var entity = db.Configuration.FirstOrDefault(x => x.Id.Equals(cmd.Id));

                    if (entity == null)
                    {
                        await transaction.RollbackAsync();
                        return;
                    }

                    db.Remove(entity);
                    db.Upsert(new ProcessedEvents(cmd.Id, "ConfigurationBasicProjection", cmd.EventId, DateTime.UtcNow));

                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        ReceiveAsync<ConfigurationUpdated>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            var processedEvent = db.ProcessedEvents.Find(cmd.Id, cmd.EventId, "ConfigurationBasicProjection");

            if (processedEvent != null)
                return;
            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();

                try
                {
                    var entity = db.Configuration.FirstOrDefault(x => x.Id.Equals(cmd.Id));

                    if (entity == null)
                    {
                        await transaction.RollbackAsync();
                        return;
                    }

                    entity.Name = cmd.Name;
                    entity.Value = cmd.Value;
                    entity.Description = cmd.Description;
                    entity.StartDate = cmd.StartDate;
                    entity.ExpireDate = cmd.ExpireDate;

                    db.Upsert(entity);
                    db.Upsert(new ProcessedEvents(cmd.Id, "ConfigurationBasicProjection", cmd.EventId, DateTime.UtcNow));

                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        Receive<DomainEvent>(evt =>
        {
            switch (evt)
            {
                case ConfigurationCreated:
                case ConfigurationUpdated:
                case ConfigurationDeactivated:
                case ConfigurationDeleted:
                    Self.Tell(evt);
                    break;

                default:
                    break;
            }
        });
    }
}
