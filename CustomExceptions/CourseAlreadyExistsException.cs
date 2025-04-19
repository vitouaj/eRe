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