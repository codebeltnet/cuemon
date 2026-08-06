using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides a default implementation of the <see cref="IServerTiming"/> interface.
/// </summary>
/// <seealso cref="IServerTiming" />
public class ServerTiming : IServerTiming
{
    /// <summary>
    /// The name of the Server-Timing header field.
    /// </summary>
    public const string HeaderName = "Server-Timing";

    private readonly List<ServerTimingMetric> _metrics = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTiming"/> class.
    /// </summary>
    public ServerTiming()
    {
    }

    /// <summary>
    /// Adds a <see cref="ServerTimingMetric" /> to the <see cref="Metrics" />.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public IServerTiming AddServerTiming(string name)
    {
        _metrics.Add(new ServerTimingMetric(name));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="ServerTimingMetric" /> to the <see cref="Metrics" />.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <param name="duration">The server-specified metric duration.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public IServerTiming AddServerTiming(string name, TimeSpan duration)
    {
        _metrics.Add(new ServerTimingMetric(name, duration));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="ServerTimingMetric" /> to the <see cref="Metrics" />.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <param name="duration">The server-specified metric duration.</param>
    /// <param name="description">The server-specified metric description.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public IServerTiming AddServerTiming(string name, TimeSpan duration, string description)
    {
        _metrics.Add(new ServerTimingMetric(name, duration, description));
        return this;
    }

    /// <summary>
    /// Gets the entries used to communicate one or more metrics and descriptions for the given request-response cycle.
    /// </summary>
    /// <value>The entries used to communicate one or more metrics and descriptions for the given request-response cycle.</value>
    public IEnumerable<ServerTimingMetric> Metrics => _metrics;
}
