using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Provides a base-class for exceptions based on an HTTP status code.
/// </summary>
/// <seealso cref="Exception" />
public abstract class HttpStatusCodeException : Exception
{
    /// <summary>
    /// Attempts to resolve a suitable <paramref name="httpStatusCodeException" /> from the specified <paramref name="statusCode" />.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="httpStatusCodeException">When this method returns, contains the by <paramref name="statusCode"/> resolved <see cref="HttpStatusCodeException"/>, or <c>null</c> if no matches could be made.</param>
    /// <returns><c>true</c> if an instance of <see cref="HttpStatusCodeException"/> could be resolved, <c>false</c> otherwise.</returns>
    public static bool TryParse(int statusCode, out HttpStatusCodeException httpStatusCodeException)
    {
        return TryParse(statusCode, null, out httpStatusCodeException);
    }

    /// <summary>
    /// Attempts to resolve a suitable <paramref name="httpStatusCodeException" /> from the specified <paramref name="statusCode" />.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="httpStatusCodeException">When this method returns, contains the by <paramref name="statusCode"/> resolved <see cref="HttpStatusCodeException"/>, or <c>null</c> if no matches could be made.</param>
    /// <returns><c>true</c> if an instance of <see cref="HttpStatusCodeException"/> could be resolved, <c>false</c> otherwise.</returns>
    public static bool TryParse(int statusCode, string message, out HttpStatusCodeException httpStatusCodeException)
    {
        return TryParse(statusCode, message, null, out httpStatusCodeException);
    }

    /// <summary>
    /// Attempts to resolve a suitable <paramref name="httpStatusCodeException" /> from the specified <paramref name="statusCode" />.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="httpStatusCodeException">When this method returns, contains the by <paramref name="statusCode"/> resolved <see cref="HttpStatusCodeException"/>, or <c>null</c> if no matches could be made.</param>
    /// <returns><c>true</c> if an instance of <see cref="HttpStatusCodeException"/> could be resolved, <c>false</c> otherwise.</returns>
    public static bool TryParse(int statusCode, string message, Exception innerException, out HttpStatusCodeException httpStatusCodeException)
    {
        var success = true;
        switch (statusCode)
        {
            case StatusCodes.Status400BadRequest:
                httpStatusCodeException = new BadRequestException(message, innerException);
                break;
            case StatusCodes.Status401Unauthorized:
                httpStatusCodeException = new UnauthorizedException(message, innerException);
                break;
            case StatusCodes.Status403Forbidden:
                httpStatusCodeException = new ForbiddenException(message, innerException);
                break;
            case StatusCodes.Status404NotFound:
                httpStatusCodeException = new NotFoundException(message, innerException);
                break;
            case StatusCodes.Status405MethodNotAllowed:
                httpStatusCodeException = new MethodNotAllowedException(message, innerException);
                break;
            case StatusCodes.Status406NotAcceptable:
                httpStatusCodeException = new NotAcceptableException(message, innerException);
                break;
            case StatusCodes.Status409Conflict:
                httpStatusCodeException = new ConflictException(message, innerException);
                break;
            case StatusCodes.Status410Gone:
                httpStatusCodeException = new GoneException(message, innerException);
                break;
            case StatusCodes.Status412PreconditionFailed:
                httpStatusCodeException = new PreconditionFailedException(message, innerException);
                break;
            case StatusCodes.Status413PayloadTooLarge:
                httpStatusCodeException = new PayloadTooLargeException(message, innerException);
                break;
            case StatusCodes.Status415UnsupportedMediaType:
                httpStatusCodeException = new UnsupportedMediaTypeException(message, innerException);
                break;
            case StatusCodes.Status428PreconditionRequired:
                httpStatusCodeException = new PreconditionRequiredException(message, innerException);
                break;
            case StatusCodes.Status429TooManyRequests:
                httpStatusCodeException = new TooManyRequestsException(message, innerException);
                break;
            default:
                success = false;
                httpStatusCodeException = null;
                break;
        }
        return success;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpStatusCodeException"/> class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    protected HttpStatusCodeException(int statusCode, string message, Exception innerException = null) : this(statusCode, Patterns.InvokeOrDefault(() => ReasonPhrases.GetReasonPhrase(statusCode), string.Empty), message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpStatusCodeException" /> class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="reasonPhrase">The HTTP reason phrase to associate with <paramref name="statusCode" /> and this exception.</param>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    protected HttpStatusCodeException(int statusCode, string reasonPhrase, string message, Exception innerException = null) : base(message, innerException)
    {
        Validator.ThrowIfLowerThan(statusCode, StatusCodes.Status100Continue, nameof(statusCode), string.Create(CultureInfo.InvariantCulture, $"{nameof(statusCode)} cannot be less than {StatusCodes.Status100Continue}."));
        Validator.ThrowIfGreaterThan(statusCode, StatusCodes.Status511NetworkAuthenticationRequired, nameof(statusCode), string.Create(CultureInfo.InvariantCulture, $"{nameof(statusCode)} cannot be greater than {StatusCodes.Status511NetworkAuthenticationRequired}."));
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
    }

    /// <summary>
    /// Gets the collection of HTTP response headers.
    /// </summary>
    /// <value>The collection of HTTP response headers.</value>
    public IHeaderDictionary Headers { get; } = new HeaderDictionary();

    /// <summary>
    /// Gets the HTTP status code associated with this exception.
    /// </summary>
    /// <value>The HTTP status code associated with this exception.</value>
    public int StatusCode { get; }

    /// <summary>
    /// Gets the HTTP reason phrase associated with this exception.
    /// </summary>
    /// <value>The HTTP reason phrase associated with this exception.</value>
    public string ReasonPhrase { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(base.ToString());
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("Additional Information:");
        foreach (var pi in this.GetType().GetProperties().Where(pi => pi.CanRead && Decorator.Enclose(pi.DeclaringType).HasTypes(typeof(HttpStatusCodeException))))
        {
            var value = Patterns.InvokeOrDefault(() => pi.GetValue(this, null)); // we cannot risk exceptions being thrown in a ToString method
            if (value != null) { sb.AppendLine(CultureInfo.InvariantCulture, $"{Alphanumeric.Tab}{pi.Name}: {value}"); }
        }
        return sb.ToString();
    }
}
