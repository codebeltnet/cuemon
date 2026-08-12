using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the origin server requires the request to be conditional.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class PreconditionRequiredException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionRequiredException"/> class.
    /// </summary>
    public PreconditionRequiredException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionRequiredException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PreconditionRequiredException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreconditionRequiredException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PreconditionRequiredException(string message, Exception innerException = null) : base(StatusCodes.Status428PreconditionRequired, message ?? "No conditional request-header fields was supplied to the server.", innerException)
    {
    }
}
