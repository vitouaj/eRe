namespace ERE.CustomExceptions;

public class UserAlreadyExistException : Exception
{
    private static string _message = "User already exists";
    public UserAlreadyExistException() : base(_message)
    {
    }
}
