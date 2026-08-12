using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when a request conflicts with the current state of the server.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class ConflictException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    public ConflictException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(string message, Exception innerException = null) : base(StatusCodes.Status409Conflict, message ?? "The request could not be completed due to a conflict with the current state of the resource.", innerException)
    {
    }
}
