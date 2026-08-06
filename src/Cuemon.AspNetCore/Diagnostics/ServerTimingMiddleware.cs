using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides a Server-Timing middleware implementation for ASP.NET Core.
/// </summary>
public class ServerTimingMiddleware : Middleware<ILogger<ServerTimingMiddleware>, IHostEnvironment, IServerTiming, IOptions<ServerTimingOptions>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTimingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    public ServerTimingMiddleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="ServerTimingMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The <see cref="ILogger"/> used in combination with <see cref="ServerTimingOptions.LogLevelSelector"/>.</param>
    /// <param name="di2">The dependency injected <see cref="IHostEnvironment"/> of <see cref="InvokeAsync"/>.</param>
    /// <param name="di3">The dependency injected <see cref="IServerTiming"/> of <see cref="InvokeAsync"/>.</param>
    /// <param name="di4">The dependency injected <see cref="IOptions{TOptions}"/> of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override Task InvokeAsync(HttpContext context, ILogger<ServerTimingMiddleware> di1, IHostEnvironment di2, IServerTiming di3, IOptions<ServerTimingOptions> di4)
    {
        var options = di4.Value;
        var serverTiming = di3;
        var logger = di1;
        var environment = di2;
        context.Response.OnStarting(() =>
        {
            if (serverTiming != null)
            {
                var serverTimingMetrics = serverTiming.Metrics.ToList();
                if (!options.SuppressHeaderPredicate(environment)) { context.Response.Headers.Append(ServerTiming.HeaderName, serverTimingMetrics.Select(metric => metric.ToString()).ToArray()); }
                if (logger != null && options.LogLevelSelector != null)
                {
                    foreach (var metric in serverTimingMetrics)
                    {
                        var logLevel = options.LogLevelSelector(metric);
                        if (logger.IsEnabled(logLevel))
                        {
                            logger.Log(logLevel, "ServerTimingMetric {{ Name: {Name}, Duration: {Duration}ms, Description: \"{Description}\" }}",
                                metric.Name,
                                metric.Duration?.TotalMilliseconds.ToString("F1", CultureInfo.InvariantCulture) ?? 0.ToString("F1", CultureInfo.InvariantCulture),
                                metric.Description ?? "N/A");
                        }
                    }
                }
            }
            return Task.CompletedTask;
        });
        return Next(context);
    }
}
