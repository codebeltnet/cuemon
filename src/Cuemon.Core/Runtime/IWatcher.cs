using System;

namespace Cuemon.Runtime;
/// <summary>
/// Specifies that this object supports a way to monitor a resource.
/// </summary>
public interface IWatcher : IDisposable
{
    /// <summary>
    /// Occurs when a resource has changed.
    /// </summary>
    event EventHandler<WatcherEventArgs> Changed;

    /// <summary>
    /// Gets the time when the resource being monitored was last changed.
    /// </summary>
    /// <value>The time when the resource being monitored was last changed.</value>
    DateTime UtcLastModified { get; }

    /// <summary>
    /// Starts the monitoring of this <see cref="IWatcher"/> implementation.
    /// </summary>
    void StartMonitoring();
}
