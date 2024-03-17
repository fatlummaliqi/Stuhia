using Stuhia.Context;

namespace Stuhia.Core;

internal class InternalEventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IApplicationEvent
    {
        if (@event == null)
        {
            if (EventContext.Current.SilentFailuresEnabled)
            {
                return;
            }

            var exception = new ArgumentNullException(nameof(@event));

            throw exception;
        }

        var handler = EventContext.Current.ResolveHandler<TEvent>(_serviceProvider);

        await handler.HandleAsync(@event, cancellationToken);
    }
}
