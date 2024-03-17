namespace Stuhia.Tests.Unit.Helpers.Events;

internal class PeekabooEvent : IApplicationEvent
{
    public string Property { get; set; }
}
