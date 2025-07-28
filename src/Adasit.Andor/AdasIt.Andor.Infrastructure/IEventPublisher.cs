namespace AdasIt.Andor.Infrastructure;

public interface IEventPublisher
{
    Task PublishAsync(object @event, CancellationToken cancellationToken);
    Task SubscribeAsync(Action<object> handler, CancellationToken cancellationToken);
    Task UnsubscribeAsync(Action<object> handler, CancellationToken cancellationToken);
}
