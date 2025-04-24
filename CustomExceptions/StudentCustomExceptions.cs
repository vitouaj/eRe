namespace ERE.CustomExceptions;

[Serializable]
public class EnrollmentNotFoundException : Exception
{
    public EnrollmentNotFoundException()
    {
    }

    public EnrollmentNotFoundException(string? message) : base(message)
    {
    }

    public EnrollmentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class EnrollmentAlreadyExistsException : Exception
{
    public EnrollmentAlreadyExistsException()
    {
    }

    public EnrollmentAlreadyExistsException(string? message) : base(message)
    {
    }

    public EnrollmentAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class CourseNotFoundException : Exception
{
    public CourseNotFoundException()
    {
    }

    public CourseNotFoundException(string? message) : base(message)
    {
    }

    public CourseNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class StudentNotFoundException : Exception
{
    public StudentNotFoundException()
    {
    }

    public StudentNotFoundException(string? message) : base(message)
    {
    }

    public StudentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}