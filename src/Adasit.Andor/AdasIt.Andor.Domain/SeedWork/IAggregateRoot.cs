using AdasIt.Andor.Domain.Events;
using System.Collections.Generic;

namespace AdasIt.Andor.Domain.SeedWork;

public interface IAggregateRoot
{
    IReadOnlyCollection<DomainEvent> Events { get; }

    void ClearEvents();
}
