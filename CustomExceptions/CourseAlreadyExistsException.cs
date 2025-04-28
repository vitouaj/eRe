[Serializable]
public class CourseAlreadyExistsException : Exception
{
    private const string DefaultMessage = "Course already exists";
    public CourseAlreadyExistsException() : base(DefaultMessage)
    {
    }

    public CourseAlreadyExistsException(string? message) : base(message)
    {
    }

    public CourseAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class MainReportNotFoundException : Exception
{
    public MainReportNotFoundException()
    {
    }

    public MainReportNotFoundException(string? message) : base(message)
    {
    }

    public MainReportNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class ContactAlreadyExistException : Exception
{
    public ContactAlreadyExistException()
    {
    }

    public ContactAlreadyExistException(string? message) : base(message)
    {
    }

    public ContactAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}