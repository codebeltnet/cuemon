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

Console.WriteLine($"Correlation header: {headers["X-Correlation-Id"]}");
Console.WriteLine($"Server header: {message.Headers.GetValues("X-Server-Id").First()}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8f3115f05ecc6f1191b7ac85"><a href="~/run-actions/8f3115f05ecc6f1191b7ac85/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8f3115f05ecc6f1191b7ac85/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
