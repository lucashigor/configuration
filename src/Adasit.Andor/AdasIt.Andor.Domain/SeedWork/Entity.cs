using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AdasIt.Andor.Domain.SeedWork;

public abstract class Entity<TEntityId> where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    public TEntityId Id { get; protected init; } = TEntityId.New();

    private readonly ICollection<Notification> _notifications = [];
    protected IReadOnlyCollection<Notification> Notifications => [.. _notifications];

    private readonly ICollection<Notification> _warnings = [];
    protected IReadOnlyCollection<Notification> Warnings => [.. _warnings];

    protected DomainResult Validate()
    {
        AddNotification(Id!.NotNull());

        return Notifications.Count != 0
            ? DomainResult.Failure(errors: _notifications)
            : DomainResult.Success(warnings: _warnings);
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
    
    protected async Task<(DomainResult, REntity?)> ValidateAsync<REntity>(
        IDefaultValidator<REntity, TEntityId> validator,
        CancellationToken cancellationToken)
        where REntity : Entity<TEntityId>
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this is not REntity entity)
            throw new InvalidOperationException($"Entity is not of type {typeof(REntity).Name}");

        var notifications = await validator.ValidateCreationAsync(entity, cancellationToken);

        AddNotification(notifications);
        AddNotification(Id!.NotNull());

        var domainResult = Notifications.Count != 0
            ? DomainResult.Failure(errors: _notifications)
            : DomainResult.Success(warnings: _warnings);

        return domainResult.IsFailure
            ? (domainResult, null)
            : (domainResult, entity);
    }
}