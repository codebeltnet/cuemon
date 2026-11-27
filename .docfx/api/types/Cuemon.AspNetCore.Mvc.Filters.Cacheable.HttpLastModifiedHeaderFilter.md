---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter"/> executes `OnResultExecutionAsync` for an `IEntityDataTimestamp` result and emits the `Last-Modified` response header.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await HttpLastModifiedHeaderFilterExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class HttpLastModifiedHeaderFilterExample
    {
        public static async Task DemonstrateAsync()
        {
            var filter = new HttpLastModifiedHeaderFilter();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Get;

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var payload = new TimestampedResource(
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 1, 2, 12, 0, 0, DateTimeKind.Utc));

            var resultContext = new ResultExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new ObjectResult(payload),
                controller: null!);

            await filter.OnResultExecutionAsync(resultContext, () =>
                Task.FromResult(new ResultExecutedContext(actionContext, new List<IFilterMetadata>(), resultContext.Result, controller: null!)));

            Console.WriteLine($"HasLastModifiedProvider: {filter.Options.HasLastModifiedProvider}");
            Console.WriteLine($"Last-Modified: {httpContext.Response.Headers[HeaderNames.LastModified]}");
        }

        private sealed record TimestampedResource(DateTime Created, DateTime? Modified) : IEntityDataTimestamp;
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="07cab98c3a10fe96457d50cf"><a href="~/run-actions/07cab98c3a10fe96457d50cf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/07cab98c3a10fe96457d50cf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
