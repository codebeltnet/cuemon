using System;
using Cuemon.Configuration;

namespace Cuemon.Runtime;
/// <summary>
/// Configuration options for <see cref="IWatcher"/>.
/// </summary>
public class WatcherOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WatcherOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="WatcherOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="DueTime"/></term>
    ///         <description><c>TimeSpan.Zero</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="DueTimeOnChanged"/></term>
    ///         <description><c>TimeSpan.Zero</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Period"/></term>
    ///         <description><c>TimeSpan.FromMinutes(2)</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public WatcherOptions()
    {
        DueTime = TimeSpan.Zero;
        DueTimeOnChanged = TimeSpan.Zero;
        Period = TimeSpan.FromMinutes(2);
    }

    /// <summary>
    /// Gets or sets the <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="IWatcher"/> starts signaling.
    /// </summary>
    /// <value>A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="IWatcher"/> starts signaling.</value>
    /// <remarks>Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</remarks>
    public TimeSpan DueTime { get; set; }


    /// <summary>
    /// Gets or sets the amount of time to postpone a <see cref="IWatcher.Changed"/> event.
    /// </summary>
    /// <value>A <see cref="TimeSpan"/> representing the amount of time to postpone a <see cref="IWatcher.Changed"/> event.</value>
    /// <remarks>Specify zero (0) to disable postponing.</remarks>
    public TimeSpan DueTimeOnChanged { get; set; }

    /// <summary>
    /// Gets or sets the time interval between periodic signaling.
    /// </summary>
    /// <value>A <see cref="TimeSpan"/> representing the time interval between periodic signaling.</value>
    /// <remarks>Specify negative one (-1) milliseconds to disable periodic signaling.</remarks>
    public TimeSpan Period { get; set; }
}
