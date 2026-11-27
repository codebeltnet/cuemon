---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and sets up a `ServiceCollection` with `ApiKeySentinelOptions` (configuring `"secret-key"` in `AllowedKeys`) and `ApiKeySentinelFilter`. It resolves the filter from <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute"/> via `CreateInstance` and invokes `OnAuthorizationAsync` on two `AuthorizationFilterContext` instances. As the outcome, the request without `X-Api-Key` receives a `ContentResult` rejection, while the request with a valid key is authorized (`Result` remains `null`).

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Examples;

await ApiKeySentinelAttributeExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class ApiKeySentinelAttributeExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<ApiKeySentinelOptions>(o => o.AllowedKeys.Add("secret-key"));
            services.AddTransient<ApiKeySentinelFilter>();
            using var provider = services.BuildServiceProvider();

            var attribute = new ApiKeySentinelAttribute();
            var filter = (IAsyncAuthorizationFilter)attribute.CreateInstance(provider);

            var unauthContext = new AuthorizationFilterContext(
                new ActionContext(new DefaultHttpContext { RequestServices = provider }, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>());
            await filter.OnAuthorizationAsync(unauthContext);
            Console.WriteLine($"Without API key result: {unauthContext.Result?.GetType().Name}");

            var authHttpContext = new DefaultHttpContext { RequestServices = provider };
            authHttpContext.Request.Headers["X-Api-Key"] = "secret-key";
            var authContext = new AuthorizationFilterContext(
                new ActionContext(authHttpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>());
            await filter.OnAuthorizationAsync(authContext);
            Console.WriteLine($"With valid API key authorized: {authContext.Result is null}");
        }
    }

    [ApiController]
    [Route("orders")]
    [ApiKeySentinel]
    public sealed class OrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOrders() => Ok();
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e2f263f4fa09ece34203902d"><a href="~/run-actions/e2f263f4fa09ece34203902d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e2f263f4fa09ece34203902d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
