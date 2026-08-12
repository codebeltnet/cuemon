namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// The exception that is thrown when the requirements of an API key header is not meet.
/// </summary>
/// <seealso cref="HttpStatusCodeException" />
public class ApiKeyException : HttpStatusCodeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyException"/> class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="message">The message that describes the HTTP status code.</param>
    public ApiKeyException(int statusCode, string message) : base(statusCode, message)
    {
    }
}
