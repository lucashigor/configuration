using System;
using System.Collections.Generic;

namespace AdasIt.Andor.Domain.SeedWork;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : IEquatable<T>, IId<T>
{
    protected AggregateRoot()
    {
        _events = new HashSet<object>();
    }


    private readonly ICollection<object> _events;

    public IReadOnlyCollection<object> Events => [.. _events];

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void RaiseDomainEvent(object @event)
    {
        _events.Add(@event);
    }
}

