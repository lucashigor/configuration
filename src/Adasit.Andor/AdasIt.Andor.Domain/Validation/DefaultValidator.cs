using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Domain.Validation;

public class DefaultValidator<TEntity, TEntityId> : IDefaultValidator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    
    public virtual async Task<List<Notification>> ValidateCreationAsync(TEntity entity,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = [];

        await DefaultValidationsAsync(entity, notifications, cancellationToken);

        return notifications;
    }
    
    protected virtual Task DefaultValidationsAsync(
        TEntity entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        
        return Task.CompletedTask;
    }
    
    protected static void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}