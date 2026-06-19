---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute
example:
- *content
---

The following example applies <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingAttribute"/> to a controller action and configures the attribute directly.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyApp.Examples;

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
```
