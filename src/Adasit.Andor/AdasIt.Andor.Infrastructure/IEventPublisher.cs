using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Infrastructure;

public interface IEventPublisher
{
    Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken);
    Task SubscribeAsync(Action<DomainEvent> handler, CancellationToken cancellationToken);
    Task UnsubscribeAsync(Action<DomainEvent> handler, CancellationToken cancellationToken);
}
