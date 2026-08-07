using System;
using System.Globalization;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Represents a HTTP Server-Timing header field entry to communicate one metric and description for the given request-response cycle.
/// </summary>
public class ServerTimingMetric
{
    private readonly string _metric;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTimingMetric"/> class.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <param name="duration">The server-specified metric duration.</param>
    /// <param name="description">The server-specified metric description.</param>
    public ServerTimingMetric(string name, TimeSpan? duration = null, string description = null)
    {
        Validator.ThrowIfNullOrWhitespace(name);
        if (duration.HasValue && duration <= TimeSpan.Zero) { duration = TimeSpan.Zero; }

        var metric = name;
        if (duration.HasValue) { metric = string.Concat(metric, ";", string.Create(CultureInfo.InvariantCulture, $"dur={duration.Value.TotalMilliseconds.ToString("F1", CultureInfo.InvariantCulture)}")); }
        if (description != null) { metric = string.Concat(metric, ";", $"desc=\"{description}\""); }

        Name = name;
        Duration = duration;
        Description = description;

        _metric = metric;
    }

    /// <summary>
    /// Gets the server-specified metric name.
    /// </summary>
    /// <value>The server-specified metric name.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the server-specified metric duration.
    /// </summary>
    /// <value>The server-specified metric duration.</value>
    public TimeSpan? Duration { get; }

    /// <summary>
    /// Gets the server-specified metric description.
    /// </summary>
    /// <value>The server-specified metric description.</value>
    public string Description { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return _metric;
    }
}
