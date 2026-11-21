---
uid: Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware"/> to append a `Vary: Accept` header to every response.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

var context = new DefaultHttpContext();
var middleware = new VaryAcceptMiddleware(httpContext => httpContext.Response.WriteAsync("Hello"));

await middleware.InvokeAsync(context);

// The middleware writes this header when the response starts.
Console.WriteLine($"{HeaderNames.Vary}: {HeaderNames.Accept}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d14d5072b889b7678c32d73d"><a href="~/run-actions/d14d5072b889b7678c32d73d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d14d5072b889b7678c32d73d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
