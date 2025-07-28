using AdasIt.Andor.Infrastructure;

namespace AdasIt.Andor.Configurations.Application;

public class InMemoryEventPublisher : IEventPublisher
{
    private readonly List<Action<object>> _subscribers = new();
    private readonly object _lock = new();

    public Task PublishAsync(object @event, CancellationToken cancellationToken)
    {
        List<Action<object>> subscribersCopy;

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

    public Task SubscribeAsync(Action<object> handler, CancellationToken cancellationToken)
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

    public Task UnsubscribeAsync(Action<object> handler, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            _subscribers.Remove(handler);
        }

        return Task.CompletedTask;
    }
}

