namespace ERE.CustomExceptions;

public class InvalidLoginException : Exception
{
    private static string _message = "Invalid login credentials";
    public InvalidLoginException() : base(_message)
    {
    }

    public InvalidLoginException(string message) : base(message)
    {
    }

    public InvalidLoginException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
