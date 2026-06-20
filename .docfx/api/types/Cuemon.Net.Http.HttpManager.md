---
uid: Cuemon.Net.Http.HttpManager
example:
- *content
---

`HttpManager` provides HTTP operation methods backed by a configurable `IHttpClientFactory`. This example creates an `HttpManager` with a custom `EchoHandler` that returns `200 OK` for all requests, then calls `HttpGetAsync` on a test URI. Key setup includes passing a factory delegate that returns `HttpClient` instances backed by the echo handler. Console output confirms the response status code (`OK`) and that `Timeout` is greater than `TimeSpan.Zero`.

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
