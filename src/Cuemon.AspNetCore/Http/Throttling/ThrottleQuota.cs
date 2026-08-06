using System;

namespace Cuemon.AspNetCore.Http.Throttling;
/// <summary>
/// Specifies the allowed quota and window duration of HTTP requests.
/// </summary>
public class ThrottleQuota
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottleQuota"/> class.
    /// </summary>
    /// <param name="rateLimit">The allowed rate from within a given <paramref name="window"/>.</param>
    /// <param name="window">The duration of the window.</param>
    /// <param name="windowUnit">One of the enumeration values that specifies the time unit of <paramref name="window"/>.</param>
    public ThrottleQuota(int rateLimit, double window, TimeUnit windowUnit) : this(rateLimit, Decorator.Enclose(window).ToTimeSpan(windowUnit))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottleQuota"/> class.
    /// </summary>
    /// <param name="rateLimit">The allowed rate from within a given <paramref name="window"/>.</param>
    /// <param name="window">The duration of the window.</param>
    public ThrottleQuota(int rateLimit, TimeSpan window)
    {
        RateLimit = rateLimit;
        Window = window;
    }

    /// <summary>
    /// Gets the allowed rate before throttling.
    /// </summary>
    /// <value>The allowed rate before throttling.</value>
    public int RateLimit { get; }

    /// <summary>
    /// Gets the allowed window duration before throttling.
    /// </summary>
    /// <value>The allowed window duration before throttling.</value>
    public TimeSpan Window { get; }
}
