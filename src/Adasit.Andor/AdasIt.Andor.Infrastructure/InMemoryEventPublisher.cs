using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Infrastructure;

public class InMemoryEventPublisher : IEventPublisher
{
    private readonly List<Action<DomainEvent>> _subscribers = new();
    private readonly DomainEvent _lock = new();

    public Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken)
    {
        List<Action<DomainEvent>> subscribersCopy;

        lock (_lock)
        {
            subscribersCopy = _subscribers.ToList();
        }

        foreach (var handler in subscribersCopy)
        {
            handler(@event);
        }

        return Task.CompletedTask;
    }

    public Task SubscribeAsync(Action<DomainEvent> handler, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (!_subscribers.Contains(handler))
            {
                _subscribers.Add(handler);
            }
        }

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(Action<DomainEvent> handler, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            _subscribers.Remove(handler);
        }

        return Task.CompletedTask;
    }
}
