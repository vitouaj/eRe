[Serializable]
public class TeacherNotFoundException : Exception
{
    private const string DefaultMessage = "Teacher not found";
    public TeacherNotFoundException() : base(DefaultMessage)
    {
    }

    public TeacherNotFoundException(string? message) : base(message)
    {
    }

    public TeacherNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class CourseReportAlreadyExistsException : Exception
{
    public CourseReportAlreadyExistsException()
    {
    }

    public CourseReportAlreadyExistsException(string? message) : base(message)
    {
    }

    public CourseReportAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}