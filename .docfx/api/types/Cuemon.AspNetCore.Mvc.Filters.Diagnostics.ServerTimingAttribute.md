---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and sets up a `ServiceCollection` with logging, `IHostEnvironment`, and `IServerTiming`. It applies <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute"/> to `WeatherController.GetWeatherAsync`, creates the filter via `CreateInstance`, and executes `OnActionExecuting` and `OnActionExecuted` against an MVC action context. As the outcome, the response emits a `Server-Timing` header containing the `weatherApi` metric.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyApp.Examples;

await ServerTimingAttributeExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class ServerTimingAttributeExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IHostEnvironment, SampleHostEnvironment>();
            services.AddSingleton<IServerTiming, ServerTiming>();
            using var provider = services.BuildServiceProvider();

            var attribute = new ServerTimingAttribute
            {
                Name = "weatherApi",
                Description = "Weather API endpoint",
                Threshold = 500,
                ThresholdTimeUnit = TimeUnit.Milliseconds,
                DesiredLogLevel = LogLevel.Warning,
                EnvironmentName = string.Empty
            };

            var filter = (ServerTimingFilter)attribute.CreateInstance(provider);

            var httpContext = new DefaultHttpContext { RequestServices = provider };
            var actionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(WeatherController).GetTypeInfo(),
                MethodInfo = typeof(WeatherController).GetMethod(nameof(WeatherController.GetWeatherAsync))!
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), actionDescriptor);
            var filters = new List<IFilterMetadata> { filter };

            var executingContext = new ActionExecutingContext(actionContext, filters, new Dictionary<string, object?>(), new WeatherController());
            filter.OnActionExecuting(executingContext);
            await Task.Delay(25);
            var executedContext = new ActionExecutedContext(actionContext, filters, executingContext.Controller);
            filter.OnActionExecuted(executedContext);

            Console.WriteLine($"Attribute Name: {attribute.Name}");
            Console.WriteLine($"IsReusable: {attribute.IsReusable}");
            Console.WriteLine($"{ServerTiming.HeaderName}: {httpContext.Response.Headers[ServerTiming.HeaderName]}");
        }

        private sealed class SampleHostEnvironment : IHostEnvironment
        {
            public string ApplicationName { get; set; } = nameof(ServerTimingAttributeExample);
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
            public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
            public string EnvironmentName { get; set; } = Environments.Development;
        }
    }

    [ApiController]
    [Route("weather")]
    public sealed class WeatherController : ControllerBase
    {
        [HttpGet]
        [ServerTiming(
            Name = "weatherApi",
            Description = "Weather API endpoint",
            Threshold = 500,
            ThresholdTimeUnit = TimeUnit.Milliseconds,
            DesiredLogLevel = LogLevel.Warning)]
        public async Task<IActionResult> GetWeatherAsync()
        {
            await Task.Delay(300);
            return Ok(new { Temperature = 22, Condition = "Sunny" });
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1c18c6cd5bc28f9f85a21eb8"><a href="~/run-actions/1c18c6cd5bc28f9f85a21eb8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1c18c6cd5bc28f9f85a21eb8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
