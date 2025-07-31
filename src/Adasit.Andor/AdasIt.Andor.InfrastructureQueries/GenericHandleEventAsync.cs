using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.InfrastructureQueries
{
    public static class GenericHandleEventAsync
    {
        public static async Task HandleEventAsync<T>(
            string projectionName,
            IServiceProvider serviceProvider,
            Guid id, Guid eventId, Func<T, Task<bool>> handler)
            where T : PrincipalContext
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<T>();

            if (await db.ProcessedEvents.FindAsync(id, eventId, projectionName) != null)
                return;

            var strategy = db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();
                try
                {
                    var shouldCommit = await handler(db);

                    if (!shouldCommit)
                    {
                        await transaction.RollbackAsync();
                        return;
                    }

                    db.Upsert(new ProcessedEvents(id, projectionName, eventId, DateTime.UtcNow));
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
