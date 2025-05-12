namespace Shared;

public class NotException : Exception
{
    
    public NotException()
    {
    }

    public NotException(string message)
        : base(message)
    {
    }

    public NotException(string message, Exception inner)
        : base(message, inner)
    {
    }
}