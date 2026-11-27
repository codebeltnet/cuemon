---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add a request rate limiting middleware to an ASP.NET Core application pipeline and invoke it to observe the rate-limit response headers.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();
builder.Services.AddLogging();
builder.Services.AddMemoryThrottlingCache();
builder.Services.AddThrottlingSentinelOptions(options =>
{
    options.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
    options.ContextResolver = context => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
});

await using var app = builder.Build();
app.Urls.Add("http://127.0.0.1:0");

app.UseThrottlingSentinel();
app.Run(context => context.Response.WriteAsync("ok"));

await app.StartAsync();
try
{
    using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
    using var response = await client.GetAsync("/");
    Console.WriteLine($"Status: {(int)response.StatusCode}");
    foreach (var header in response.Headers.Where(h => h.Key.Contains("RateLimit", StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
    }
}
finally
{
    await app.StopAsync();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ba5caf13fa30031782043b17"><a href="~/run-actions/ba5caf13fa30031782043b17/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ba5caf13fa30031782043b17/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
