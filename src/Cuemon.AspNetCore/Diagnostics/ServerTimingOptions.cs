using System;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Configuration options for <see cref="ServerTimingMiddleware"/> and related.
/// </summary>
/// <seealso cref="TimeMeasureOptions" />
public class ServerTimingOptions : TimeMeasureOptions, IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTimingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ServerTimingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="TimeMeasureOptions.TimeMeasureCompletedThreshold"/></term>
    ///         <description><see cref="TimeSpan.Zero"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="LogLevelSelector"/></term>
    ///         <description><c>metric => metric.Duration.HasValue ? LogLevel.Debug : LogLevel.None</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SuppressHeaderPredicate"/></term>
    ///         <description><c>environment => environment.IsProduction()</c></description>
    ///     </item>
    /// 		///     <item>
    ///         <term><see cref="UseTimeMeasureProfiler"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ServerTimingOptions()
    {
        LogLevelSelector = metric => metric.Duration.HasValue ? LogLevel.Debug : LogLevel.None;
        SuppressHeaderPredicate = environment => environment.IsProduction();
    }

    /// <summary>
    /// Gets or sets the predicate that can suppress the Server-Timing HTTP header(s).
    /// </summary>
    /// <value>The function delegate that can determine if the Server-Timing HTTP header(s) should be suppressed.</value>
    public Func<IHostEnvironment, bool> SuppressHeaderPredicate { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that determines the <see cref="LogLevel"/> for a given <see cref="ServerTimingMetric"/>.
    /// </summary>
    /// <value>The function delegate that determines the <see cref="LogLevel"/> for a given <see cref="ServerTimingMetric"/>.</value>
    public Func<ServerTimingMetric, LogLevel> LogLevelSelector { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply <see cref="TimeMeasureProfiler"/> automatically on action methods in a Controller.
    /// </summary>
    /// <value><c>true</c> if action methods in a Controller should time measuring automatically; otherwise, <c>false</c>.</value>
    /// <remarks>This property is only used in the context of a Global Filter for MVC.</remarks>
    public bool UseTimeMeasureProfiler { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="SuppressHeaderPredicate"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(SuppressHeaderPredicate == null);
        Validator.ThrowIfInvalidState(LogLevelSelector == null);
    }
}
