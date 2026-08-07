using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the request entity is larger than limits defined by server.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class PayloadTooLargeException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
    /// </summary>
    public PayloadTooLargeException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PayloadTooLargeException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PayloadTooLargeException(string message, Exception innerException = null) : base(StatusCodes.Status413PayloadTooLarge, message ?? "The server is refusing to process a request because the request entity is larger than the server is willing or able to process.", innerException)
    {
    }
}
