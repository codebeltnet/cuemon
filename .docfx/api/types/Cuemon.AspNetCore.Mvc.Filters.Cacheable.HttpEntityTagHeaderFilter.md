---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter"/> with `UseEntityTagResponseParser = true`. After setting up a `DefaultHttpContext` with a `MemoryStream` response body and `ResultExecutingContext` containing an `ObjectResult`, it invokes `OnResultExecutionAsync`. As the outcome, the filter hashes the written response body and emits a matching `ETag` response header.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

var filter = new HttpEntityTagHeaderFilter(o => o.UseEntityTagResponseParser = true);

var httpContext = new DefaultHttpContext();
httpContext.Request.Method = HttpMethods.Get;
httpContext.Response.Body = new MemoryStream();

var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
var resultContext = new ResultExecutingContext(
    actionContext,
    new List<IFilterMetadata>(),
    new ObjectResult("Hello, ETag!"),
    controller: null!);

await filter.OnResultExecutionAsync(resultContext, async () =>
{
    await httpContext.Response.WriteAsync("Hello, ETag!");
    return new ResultExecutedContext(actionContext, new List<IFilterMetadata>(), resultContext.Result, controller: null!);
});

Console.WriteLine($"UseEntityTagResponseParser: {filter.Options.UseEntityTagResponseParser}");
Console.WriteLine($"ETag: {httpContext.Response.Headers[HeaderNames.ETag]}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2ce0b3fe0f1703c5e548ae0a"><a href="~/run-actions/2ce0b3fe0f1703c5e548ae0a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2ce0b3fe0f1703c5e548ae0a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
