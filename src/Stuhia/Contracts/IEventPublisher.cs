namespace Stuhia;

/// <summary>
/// Represents a service for publishing application level events asynchronously.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes the specified event asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish, must implement <see cref="IApplicationEvent"/>.</typeparam>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IApplicationEvent;
}
