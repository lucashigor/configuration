using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;
using System;
using System.Collections.Generic;

namespace AdasIt.Andor.Domain.SeedWork;

public abstract class Entity<TEntityId> where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    public TEntityId Id { get; protected set; }

    protected readonly ICollection<Notification> _notifications;
    protected IReadOnlyCollection<Notification> Notifications => [.. _notifications];

    protected readonly ICollection<Notification> _warnings;
    protected IReadOnlyCollection<Notification> Warnings => [.. _warnings];

    protected Entity()
    {
        Id = (TEntityId)TEntityId.New();
        _notifications = [];
        _warnings = [];
    }

    protected virtual DomainResult Validate()
    {
        AddNotification(Id!.NotNull());

        if (Notifications.Count != 0)
        {
            return DomainResult.Failure(errors: _notifications);
        }

        return DomainResult.Success(warnings: _warnings);
    }

    protected void AddNotification(List<Notification> notifications)
    {
        notifications.ForEach(x => AddNotification(x));
    }

    protected void AddNotification(Notification? notification)
    {
        if (notification != null)
        {
            _notifications.Add(notification);
        }
    }

    protected void AddNotification(string fieldName, string message, DomainErrorCode domainError)
        => AddNotification(new Notification(fieldName, message, domainError));

    protected void AddWarning(Notification? notification)
    {
        if (notification != null)
        {
            _warnings.Add(notification);
        }
    }

    protected void AddWarning(string fieldName, string message, DomainErrorCode domainError)
        => AddWarning(new(fieldName, message, domainError));

}
