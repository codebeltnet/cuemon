using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// A filter that performs time measure profiling of action methods.
    /// </summary>
    /// <seealso cref="ConfigurableActionFilter{TOptions}"/>
    /// <seealso cref="IActionFilter" />
    public class ServerTimingFilter : ConfigurableActionFilter<ServerTimingOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTimingFilter" /> class.
        /// </summary>
        /// <param name="setup">The <see cref="TimeMeasureOptions" /> which need to be configured.</param>
        /// <param name="environment">The dependency injected <see cref="IHostEnvironment"/>.</param>
        /// <param name="logger">The dependency injected <see cref="ILogger{TCategoryName}"/>.</param>
        public ServerTimingFilter(IOptions<ServerTimingOptions> setup, IHostEnvironment environment, ILogger<ServerTimingFilter> logger) : base(setup)
        {
            Profiler = new TimeMeasureProfiler();
            Environment = environment;
            Logger = logger;
        }

        private ILogger<ServerTimingFilter> Logger { get; }

        private IHostEnvironment Environment { get; }

        private TimeMeasureProfiler Profiler { get; }

        internal bool FromAttributeDecoration { get; set; }  // this should only be enabled for ServerTimingAttribute

        internal string Name { get; set; }

        internal string Description { get; set; }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Options.UseTimeMeasureProfiler)
            {
                Profiler.Timer.Start();
                if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
                {
                    var expectedObjects = ParseRuntimeParameters(context, descriptor);
                    var verifiedObjects = context.ActionArguments.Values.ToArray();
                    if (verifiedObjects.Length == expectedObjects.Length) { expectedObjects = verifiedObjects; }
                    var md = Options.MethodDescriptor?.Invoke() ?? ParseMethodDescriptor(descriptor);
                    Profiler.Member = md;
                    Profiler.Data = md.MergeParameters(Options.RuntimeParameters ?? expectedObjects);
                }
            }
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext" />.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (Options.UseTimeMeasureProfiler) { Profiler.Timer.Stop(); }

            var hasGlobalFilter = context.Filters.Any(filter => filter is ServerTimingFilter serverTimingFilter && !serverTimingFilter.FromAttributeDecoration);
            var skipExecutionDueToDoubleFilterRegistration = hasGlobalFilter && FromAttributeDecoration;
            var serverTiming = context.HttpContext.RequestServices.GetRequiredService<IServerTiming>();

            if (Options.UseTimeMeasureProfiler)
            {
                serverTiming.AddServerTiming(Name ?? Decorator.Enclose(Profiler.Member.MethodName).ToAsciiEncodedString(),
                    Profiler.Elapsed,
                    Description ?? $"{context.HttpContext.Request.GetEncodedUrl().ToLowerInvariant()}");
                if (Options.TimeMeasureCompletedThreshold == TimeSpan.Zero || Profiler.Elapsed > Options.TimeMeasureCompletedThreshold)
                {
                    TimeMeasure.CompletedCallback?.Invoke(Profiler);
                }
            }

            if (skipExecutionDueToDoubleFilterRegistration) { return; }

            var serverTimingMetrics = serverTiming.Metrics.DistinctBy(metric => metric.Name).ToList();
            if (!Options.SuppressHeaderPredicate(Environment)) { context.HttpContext.Response.Headers.Append(ServerTiming.HeaderName, serverTimingMetrics.Select(metric => metric.ToString()).ToArray()); }
            if (Logger != null && Options.LogLevelSelector != null)
            {
                foreach (var metric in serverTimingMetrics)
                {
                    var logLevel = Options.LogLevelSelector(metric);
                    if (Logger.IsEnabled(logLevel))
                    {
                        Logger.Log(logLevel, "ServerTimingMetric {{ Name: {Name}, Duration: {Duration}ms, Description: \"{Description}\" }}",
                            metric.Name,
                            metric.Duration?.TotalMilliseconds.ToString("F1", CultureInfo.InvariantCulture) ?? 0.ToString("F1", CultureInfo.InvariantCulture),
                            metric.Description ?? "N/A");
                    }
                }
            }
        }

        private static MethodDescriptor ParseMethodDescriptor(ControllerActionDescriptor descriptor)
        {
            return descriptor == null ? null : new MethodDescriptor(descriptor.MethodInfo);
        }

        private static object[] ParseRuntimeParameters(FilterContext context, ControllerActionDescriptor descriptor)
        {
            if (descriptor == null) { return Array.Empty<object>(); }
            var objects = new List<object>();
            foreach (var pi in descriptor.Parameters)
            {
                if (context.RouteData.Values.TryGetValue(pi.Name, out var routeObject))
                {
                    objects.Add(Decorator.Enclose(routeObject).ChangeType(pi.ParameterType));
                    continue;
                }

                if (context.HttpContext.Request.Query[pi.Name] != StringValues.Empty)
                {
                    objects.Add(Decorator.Enclose(context.HttpContext.Request.Query[pi.Name].ToString()).ChangeType(pi.ParameterType));
                    continue;
                }

                if (context.HttpContext.Request.HasFormContentType && context.HttpContext.Request.Form[pi.Name] != StringValues.Empty)
                {
                    objects.Add(Decorator.Enclose(context.HttpContext.Request.Form[pi.Name].ToString()).ChangeType(pi.ParameterType));
                    continue;
                }

                if (context.HttpContext.Request.Headers[pi.Name] != StringValues.Empty)
                {
                    objects.Add(Decorator.Enclose(context.HttpContext.Request.Headers[pi.Name].ToString()).ChangeType(pi.ParameterType));
                }
            }
            return objects.ToArray();
        }
    }
}
