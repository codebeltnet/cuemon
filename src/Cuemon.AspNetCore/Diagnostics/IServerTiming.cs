using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Represents the Server Timing as per W3C Working Draft 28 July 2020 (https://www.w3.org/TR/2020/WD-server-timing-20200728/).
/// </summary>
public interface IServerTiming
{
    /// <summary>
    /// Adds a <see cref="ServerTimingMetric"/> to the <see cref="Metrics"/>.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IServerTiming AddServerTiming(string name);

    /// <summary>
    /// Adds a <see cref="ServerTimingMetric"/> to the <see cref="Metrics"/>.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <param name="duration">The server-specified metric duration.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IServerTiming AddServerTiming(string name, TimeSpan duration);

    /// <summary>
    /// Adds a <see cref="ServerTimingMetric"/> to the <see cref="Metrics"/>.
    /// </summary>
    /// <param name="name">The server-specified metric name.</param>
    /// <param name="duration">The server-specified metric duration.</param>
    /// <param name="description">The server-specified metric description.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IServerTiming AddServerTiming(string name, TimeSpan duration, string description);

    /// <summary>
    /// Gets the entries used to communicate one or more metrics and descriptions for the given request-response cycle.
    /// </summary>
    /// <value>The entries used to communicate one or more metrics and descriptions for the given request-response cycle.</value>
    IEnumerable<ServerTimingMetric> Metrics { get; }
}
