using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the client has indicated preconditions in its headers which the server does not meet.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class PreconditionFailedException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionFailedException"/> class.
    /// </summary>
    public PreconditionFailedException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionFailedException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PreconditionFailedException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PreconditionFailedException(string message, Exception innerException = null) : base(StatusCodes.Status412PreconditionFailed, message ?? "The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.", innerException)
    {
    }
}
