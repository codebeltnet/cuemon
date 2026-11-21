---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute
example:
- *content
---

The following example applies <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute"/> to a controller action and configures the attribute directly. It instantiates the attribute with a name, description, threshold in milliseconds, and desired log level, then applies it declaratively to a `WeatherController` GET endpoint. The attribute outputs its configuration and confirms it implements `IFilterFactory`, demonstrating how to instrument ASP.NET Core endpoints with server-timing metrics.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyApp.Examples;

ServerTimingAttributeExample.Demonstrate();

namespace MyApp.Examples
{
    public static class ServerTimingAttributeExample
    {
        public static void Demonstrate()
        {
            var attribute = new ServerTimingAttribute
            {
                Name = "weatherApi",
                Description = "Weather API endpoint",
                Threshold = 500,
                ThresholdTimeUnit = TimeUnit.Milliseconds,
                DesiredLogLevel = LogLevel.Warning,
                EnvironmentName = string.Empty
            };

            Console.WriteLine(attribute.Name);
            Console.WriteLine(attribute is IFilterFactory);
            Console.WriteLine(attribute.IsReusable);
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
<div class="docfx-run-actions" data-example-id="6c2cadee8a34aaa5ccbab5ff"><a href="~/run-actions/6c2cadee8a34aaa5ccbab5ff/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6c2cadee8a34aaa5ccbab5ff/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
