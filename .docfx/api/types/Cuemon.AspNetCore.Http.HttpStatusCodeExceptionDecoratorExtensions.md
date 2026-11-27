---
uid: Cuemon.AspNetCore.Http.HttpStatusCodeExceptionDecoratorExtensions
example:
- *content
---

The following example demonstrates how to add response headers to an `HttpStatusCodeException` using the decorator pattern.

```csharp
// Program.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Cuemon;

var exception = new NotFoundException("Resource not found.");
var headers = new HeaderDictionary
{
    { "X-Correlation-Id", new StringValues("abc-123") },
    { "X-Request-Id", new StringValues("req-456") }
};

Decorator.Enclose(exception).AddResponseHeaders(headers);

using var message = new HttpResponseMessage();
message.Headers.Add("X-Server-Id", "server-01");

Decorator.Enclose(exception).AddResponseHeaders(message.Headers);

Console.WriteLine($"Correlation header: {exception.Headers["X-Correlation-Id"]}");
Console.WriteLine($"Request header: {exception.Headers["X-Request-Id"]}");
Console.WriteLine($"Server header: {exception.Headers["X-Server-Id"]}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fa4cd9c6c02dbc53693bd26d"><a href="~/run-actions/fa4cd9c6c02dbc53693bd26d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fa4cd9c6c02dbc53693bd26d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
