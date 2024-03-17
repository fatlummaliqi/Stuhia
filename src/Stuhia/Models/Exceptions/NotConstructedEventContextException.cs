namespace Stuhia.Models.Exceptions;

public class NotConstructedEventContextException : Exception
{
    public NotConstructedEventContextException() : base("Event Context is not constructed.")
    {
    }
}
