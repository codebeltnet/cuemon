using System;

namespace Cuemon.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="TimeMeasure" /> operations.
/// </summary>
/// <seealso cref="ProfilerOptions" />
public class TimeMeasureOptions : ProfilerOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeMeasureOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="TimeMeasureOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="TimeMeasureCompletedThreshold"/></term>
    ///         <description><see cref="TimeSpan.Zero"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public TimeMeasureOptions()
    {
        TimeMeasureCompletedThreshold = DefaultTimeMeasureCompletedThreshold;
    }

    /// <summary>
    /// Gets or sets the time measuring threshold before the <see cref="TimeMeasure.CompletedCallback"/> is invoked.
    /// </summary>
    /// <value>The time measuring threshold before the <see cref="TimeMeasure.CompletedCallback"/> is invoked.</value>
    public TimeSpan TimeMeasureCompletedThreshold { get; set; }

    /// <summary>
    /// Gets or sets the default time measuring threshold before the <see cref="TimeMeasure.CompletedCallback"/> is invoked,
    /// </summary>
    /// <value>The default time measuring threshold before the <see cref="TimeMeasure.CompletedCallback"/> is invoked.</value>
    public static TimeSpan DefaultTimeMeasureCompletedThreshold { get; set; } = TimeSpan.Zero;
}
