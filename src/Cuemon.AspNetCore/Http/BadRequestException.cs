using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// The exception that is thrown when the server could not understand the request due to invalid syntax.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class BadRequestException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    public BadRequestException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BadRequestException(Exception innerException) : this(default, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BadRequestException(string message, Exception innerException = null) : base(StatusCodes.Status400BadRequest, message ?? "The request could not be understood by the server due to malformed syntax.", innerException)
    {
    }
}
