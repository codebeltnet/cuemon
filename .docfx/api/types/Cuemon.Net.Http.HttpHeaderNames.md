---
uid: Cuemon.Net.Http.HttpHeaderNames
example:
- *content
---

The following example demonstrates how to use <see cref="HttpHeaderNames"/> constants when working with HTTP request and response headers.

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpHeaderNamesExample
{
    public static async Task DemonstrateAsync()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");

        // Set request headers using the constants
        request.Headers.Add(HttpHeaderNames.Accept, "application/json");
        request.Headers.Add(HttpHeaderNames.Authorization, "Bearer token123");
        request.Headers.Add(HttpHeaderNames.UserAgent, "MyApp/1.0");
        request.Headers.Add(HttpHeaderNames.AcceptEncoding, "gzip");

        Console.WriteLine("Request headers configured:");
        foreach (var header in request.Headers)
        {
            Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
        }
    }
}
```
