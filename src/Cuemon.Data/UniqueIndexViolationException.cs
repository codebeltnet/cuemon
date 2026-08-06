using System;

namespace Cuemon.Data;
/// <summary>
/// The exception that is thrown when a unique index violation occurs from a data source.
/// </summary>
public class UniqueIndexViolationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UniqueIndexViolationException"/> class.
    /// </summary>
    public UniqueIndexViolationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniqueIndexViolationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UniqueIndexViolationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniqueIndexViolationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public UniqueIndexViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
