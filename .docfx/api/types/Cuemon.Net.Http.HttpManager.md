---
uid: Cuemon.Net.Http.HttpManager
example:
- *content
---

The following example demonstrates how to use <see cref="HttpManager" /> with a custom in-memory HTTP handler.

```csharp
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpManagerExample
{
    public static async Task DemonstrateAsync()
    {
        using var manager = new HttpManager(() => new HttpClient(new EchoHandler(), false));
        using var response = await manager.HttpGetAsync(new Uri("https://example.com/health"));

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(manager.Timeout > TimeSpan.Zero);
    }

    private sealed class EchoHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent("OK")
            });
        }
    }
}
```
