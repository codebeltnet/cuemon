using System;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
/// <summary>
/// Represents an attribute that is used to mark an action method for time measure profiling.
/// </summary>
/// <seealso cref="ActionFilterAttribute" />
public class ServerTimingAttribute : ActionFilterAttribute, IFilterFactory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTimingAttribute"/> class.
    /// </summary>
    public ServerTimingAttribute()
    {
    }

    /// <summary>
    /// Gets or sets the server-specified metric name. Defaults to the name of the action method.
    /// </summary>
    /// <value>The server-specified metric name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the server-specified metric description. Defaults the request URI of the action method.
    /// </summary>
    /// <value>The server-specified metric description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value that in combination with <see cref="ThresholdTimeUnit" /> specifies the threshold of the action method.
    /// </summary>
    /// <value>The threshold value of the action method.</value>
    public double Threshold { get; set; }

    /// <summary>
    /// Gets or sets one of the enumeration values that specifies the time unit of <see cref="Threshold"/>.
    /// </summary>
    /// <value>The <see cref="TimeUnit"/> that defines the actual <see cref="Threshold"/>.</value>
    public TimeUnit ThresholdTimeUnit { get; set; } = TimeUnit.Ticks;

    /// <summary>
    /// Gets or sets the <see cref="LogLevel"/> of server-timing metrics. Defaults to <see cref="LogLevel.Debug"/>, which means logs are written with a severity level of debug.
    /// </summary>
    /// <value>The  <see cref="LogLevel"/> of server-timing metrics.</value>
    public LogLevel DesiredLogLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Gets or sets the name of the environment to suppress the Server-Timing header from. Default is "Production".
    /// </summary>
    /// <value>The name of the environment to suppress the Server-Timing header from.</value>
    /// <remarks>To always include the Server-Timing header, set this property to <c>null</c> or an <c>empty</c> string.</remarks>
    public string EnvironmentName { get; set; } = "Production";

    /// <summary>
	/// Creates an instance of the executable filter.
	/// </summary>
	/// <param name="serviceProvider">The request <see cref="IServiceProvider" />.</param>
	/// <returns>An instance of the executable filter.</returns>
	public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
        var logger = serviceProvider.GetService<ILogger<ServerTimingFilter>>();
        var filter = new ServerTimingFilter(Options.Create(new ServerTimingOptions()
        {
            SuppressHeaderPredicate = string.IsNullOrEmpty(EnvironmentName)
            ? _ => false
            : env => env.EnvironmentName.Equals(EnvironmentName, StringComparison.OrdinalIgnoreCase),
            LogLevelSelector = metric => metric.Duration.HasValue ? DesiredLogLevel : LogLevel.None,
            TimeMeasureCompletedThreshold = Decorator.Enclose(Threshold).ToTimeSpan(ThresholdTimeUnit),
            UseTimeMeasureProfiler = true
        }), environment, logger)
        {
            Name = Name,
            Description = Description,
            FromAttributeDecoration = true
        };
        return filter;
    }

    /// <summary>
    /// Gets a value that indicates if the result of <see cref="IFilterFactory.CreateInstance(IServiceProvider)" /> can be reused across requests.
    /// </summary>
    /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
    public bool IsReusable => false;
}
