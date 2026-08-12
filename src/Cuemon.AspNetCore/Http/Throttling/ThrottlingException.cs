using System;

namespace Cuemon.AspNetCore.Http.Throttling;
/// <summary>
/// The exception that is thrown when a given request threshold has been reached and then throttled.
/// </summary>
/// <seealso cref="TooManyRequestsException" />
public class ThrottlingException : TooManyRequestsException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottlingException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the HTTP status code.</param>
    /// <param name="rateLimit">The allowed rate of requests for a given window.</param>
    /// <param name="delta">The remaining duration of a window.</param>
    /// <param name="reset">The date and time when a window is being reset.</param>
    public ThrottlingException(string message, int rateLimit, TimeSpan delta, DateTime reset) : base(message)
    {
        RateLimit = rateLimit;
        Delta = delta;
        Reset = reset;
    }

    /// <summary>
    /// Gets the allowed rate of requests for a given window.
    /// </summary>
    /// <value>The allowed rate of requests for a given window.</value>
    public int RateLimit { get; }

    /// <summary>
    /// Gets the remaining duration of a window.
    /// </summary>
    /// <value>The remaining duration of a window.</value>
    public TimeSpan Delta { get; }

    /// <summary>
    /// Gets date and time when a window is being reset.
    /// </summary>
    /// <value>The date and time when a window is being reset.</value>
    public DateTime Reset { get; }
}
