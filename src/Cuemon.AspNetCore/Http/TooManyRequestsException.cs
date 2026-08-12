using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the user has sent too many requests in a given amount of time ("rate limiting").
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class TooManyRequestsException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
    /// </summary>
    public TooManyRequestsException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public TooManyRequestsException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public TooManyRequestsException(string message, Exception innerException = null) : base(StatusCodes.Status429TooManyRequests, message ?? "The allowed number of requests has been exceeded.", innerException)
    {
    }
}
