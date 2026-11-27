---
uid: Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and shows how to return a <xref cref="Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult"/> from `RateLimitController.GetData()` and execute it against an MVC `ActionContext`. It sets up a `ServiceCollection` with logging, controllers, and `DefaultJsonTypeInfoResolver`, then calls `ExecuteResultAsync` with a `MemoryStream` response body. As the outcome, the result reports status `429` and writes the serialized JSON payload to the HTTP response.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Examples;

var result = (TooManyRequestsObjectResult)new RateLimitController().GetData();
Console.WriteLine($"Status code: {result.StatusCode}");
Console.WriteLine($"Value: {result.Value}");

var services = new ServiceCollection();
services.AddLogging();
services.AddControllers().AddJsonOptions(o =>
    o.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver());
using var provider = services.BuildServiceProvider();

var httpContext = new DefaultHttpContext { RequestServices = provider };
httpContext.Response.Body = new MemoryStream();
await result.ExecuteResultAsync(new ActionContext(httpContext, new RouteData(), new ActionDescriptor()));

httpContext.Response.Body.Position = 0;
using var reader = new StreamReader(httpContext.Response.Body);
Console.WriteLine($"HTTP status: {httpContext.Response.StatusCode}");
Console.WriteLine($"HTTP body: {await reader.ReadToEndAsync()}");

namespace MyApp.Examples
{
    public class RateLimitController : Controller
    {
        [HttpGet("/api/rate-limited")]
        public IActionResult GetData()
        {
            return new TooManyRequestsObjectResult(new
            {
                error = "API rate limit exceeded. Please try again later.",
                retryAfter = 60
            });
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4fd934bc233c41d278a792bb"><a href="~/run-actions/4fd934bc233c41d278a792bb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4fd934bc233c41d278a792bb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
