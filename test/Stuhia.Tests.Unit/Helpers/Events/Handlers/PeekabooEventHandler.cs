
namespace Stuhia.Tests.Unit.Helpers.Events.Handlers;

internal class PeekabooEventHandler : IEventHandler<PeekabooEvent>
{
    public Task HandleAsync(PeekabooEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
