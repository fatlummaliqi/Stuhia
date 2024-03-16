namespace Stuhia;

/// <summary>
/// Represents a handler for a specific type of application level event.
/// </summary>
/// <typeparam name="TEvent">The type of event to handle, must implement <see cref="IApplicationEvent"/>.</typeparam>
public interface IEventHandler<TEvent> where TEvent : IApplicationEvent
{
    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
