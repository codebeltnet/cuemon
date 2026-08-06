using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the media format of the requested data is not supported by the server, so the server is rejecting the request.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class UnsupportedMediaTypeException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
    /// </summary>
    public UnsupportedMediaTypeException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public UnsupportedMediaTypeException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public UnsupportedMediaTypeException(string message, Exception innerException = null) : base(StatusCodes.Status415UnsupportedMediaType, message ?? "The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.", innerException)
    {
    }
}
