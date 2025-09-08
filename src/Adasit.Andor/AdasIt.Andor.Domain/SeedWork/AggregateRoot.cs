using AdasIt.Andor.Domain.Events;
using System;
using System.Collections.Generic;

namespace AdasIt.Andor.Domain.SeedWork;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : IEquatable<T>, IId<T>
{
    protected AggregateRoot()
    {
        _events = new HashSet<DomainEvent>();
    }
    
    private readonly ICollection<DomainEvent> _events;

    public IReadOnlyCollection<DomainEvent> Events => [.. _events];

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void RaiseDomainEvent(DomainEvent @event)
    {
        _events.Add(@event);
    }
}

