
using Microsoft.Extensions.Logging;
using Stuhia.Context;

namespace Stuhia.Core;

internal class InternalEventPublisher(
    IServiceProvider serviceProvider,
    ILogger<InternalEventPublisher> logger) : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<InternalEventPublisher> _logger = logger;

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IApplicationEvent
    {
        if (@event == null)
        {
            if (EventContext.Current.SilentFailuresEnabled)
            {
                if (EventContext.Current.LoggingEnabled)
                {
                    _logger.LogError("Failed publishing {EventName} application event.", @event.GetType().Name);
                }

                return;
            }

            var exception = new ArgumentNullException(nameof(@event));

            if (EventContext.Current.LoggingEnabled)
            {
                _logger.LogError(exception, "Failed publishing {EventName} application event.", @event.GetType().Name);
            }

            throw exception;
        }

        var handler = EventContext.Current.ResolveHandler<TEvent>(_serviceProvider);

        await handler.HandleAsync(@event, cancellationToken);

        if (EventContext.Current.LoggingEnabled)
        {
            _logger.LogInformation("Published & handled successfully {EventName} application event", @event.GetType().Name);
        }
    }
}
