[Serializable]
internal class InvalidRoleException : Exception
{
    private const string DefaultMessage = "Invalid role";
    public InvalidRoleException() : base(DefaultMessage)
    {
    }

    public InvalidRoleException(string? message) : base(message)
    {
    }

    public InvalidRoleException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}