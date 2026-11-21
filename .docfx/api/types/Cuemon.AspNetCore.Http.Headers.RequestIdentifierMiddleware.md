---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware"/> in the ASP.NET Core pipeline to add a unique Request-ID header to every response.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

var context = new DefaultHttpContext();
var options = Options.Create(new RequestIdentifierOptions
{
    Token = new RequestToken("req-42")
});

var middleware = new RequestIdentifierMiddleware(
    httpContext => httpContext.Response.WriteAsync("Hello"),
    options);

await middleware.InvokeAsync(context);

// The middleware writes this header when the response starts.
Console.WriteLine($"{options.Value.HeaderName}: {options.Value.Token.RequestId}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="68fbb102f914b7bee45cda4b"><a href="~/run-actions/68fbb102f914b7bee45cda4b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/68fbb102f914b7bee45cda4b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
