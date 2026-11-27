---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and `Cuemon.Data.Integrity` and shows how to set up <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter"/> end-to-end. It configures `HttpCacheableOptions` with `CacheControl`, `HttpEntityTagHeaderFilter`, and `HttpLastModifiedHeaderFilter`, prepares a `DefaultHttpContext` and `ResultExecutingContext` with an `IEntityDataTimestamp` payload, and invokes `OnResultExecutionAsync`. As the outcome, the response emits `Cache-Control`, `ETag`, and `Last-Modified` headers derived from the payload and body.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await HttpCacheableFilterExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class HttpCacheableFilterExample
    {
        public static async Task DemonstrateAsync()
        {
            var options = new HttpCacheableOptions
            {
                CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(60)
                }
            };
            options.Filters.Add(new HttpEntityTagHeaderFilter(io => io.UseEntityTagResponseParser = true));
            options.Filters.Add(new HttpLastModifiedHeaderFilter());

            var filter = new HttpCacheableFilter(Options.Create(options));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Get;
            httpContext.Response.Body = new MemoryStream();

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var payload = new TimestampedPayload(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 2, 12, 0, 0, DateTimeKind.Utc));
            var resultContext = new ResultExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new ObjectResult(payload),
                controller: null!);

            await filter.OnResultExecutionAsync(resultContext, async () =>
            {
                await httpContext.Response.WriteAsync("Hello, Cacheable!");
                return new ResultExecutedContext(actionContext, new List<IFilterMetadata>(), resultContext.Result, controller: null!);
            });

            Console.WriteLine($"Cache-Control: {httpContext.Response.Headers[HeaderNames.CacheControl]}");
            Console.WriteLine($"ETag: {httpContext.Response.Headers[HeaderNames.ETag]}");
            Console.WriteLine($"Last-Modified: {httpContext.Response.Headers[HeaderNames.LastModified]}");
        }

        private sealed record TimestampedPayload(DateTime Created, DateTime? Modified) : IEntityDataTimestamp;
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a3141f124d6dc55cffaf8008"><a href="~/run-actions/a3141f124d6dc55cffaf8008/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a3141f124d6dc55cffaf8008/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
