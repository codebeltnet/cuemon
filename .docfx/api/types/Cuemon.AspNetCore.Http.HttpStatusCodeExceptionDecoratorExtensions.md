---
uid: Cuemon.AspNetCore.Http.HttpStatusCodeExceptionDecoratorExtensions
example:
- *content
---

The following example demonstrates how to add response headers to an `HttpStatusCodeException` using the decorator pattern.

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Cuemon;
namespace Examples;

public class HttpStatusCodeExceptionHeadersExample
{
    public void AddHeadersToException()
    {
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

}
}

```
