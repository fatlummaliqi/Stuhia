namespace Stuhia.Models.Exceptions;

public class UnSupportedHandlerException(string eventName) : Exception($"Event handler for {eventName} could not be found.")
{
}
