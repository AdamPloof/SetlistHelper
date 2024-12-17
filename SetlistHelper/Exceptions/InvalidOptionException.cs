namespace SetlistHelper.Exceptions;

/// <summary>
/// An exception to use when encountering problems with user provided command line options
/// that should halt exectution but fail gracefully. The message will be displayed to the user.
/// </summary>
public class InvalidOptionException : Exception {
    public InvalidOptionException() : base() {}
    public InvalidOptionException(string? message) : base(message) {}
    public InvalidOptionException(string? message, Exception? innerException) :
        base(message, innerException) {}
}
