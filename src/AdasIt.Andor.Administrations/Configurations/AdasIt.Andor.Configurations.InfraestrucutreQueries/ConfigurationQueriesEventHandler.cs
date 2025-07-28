using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.InfraestrucutreQueries;

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

            Sender.Tell(db.Configuration.Find(cmd.Id));
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

        Receive<ConfigurationDeactivated>(cmd =>
        {
            return;
        });

        Receive<ConfigurationDeleted>(cmd =>
        {
            return;
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
                    var entity = db.Configuration.Find(cmd.Id);

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

        Receive<object>(evt =>
        {
            switch (evt)
            {
                case ConfigurationCreated created:
                case ConfigurationUpdated updated:
                case ConfigurationDeactivated deactivated:
                case ConfigurationDeleted deleted:
                    Self.Tell(evt);
                    break;

                default:
                    break;
            }
        });
    }

}
