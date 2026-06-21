---
uid: Cuemon.Net.Http.HttpRequestOptions
example:
- *content
---

The following example demonstrates how to configure and send an HTTP request using `HttpRequestOptions` with the `HttpManager`.

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;
using HttpRequestOptions = Cuemon.Net.Http.HttpRequestOptions;

namespace Examples;

public class HttpRequestExample
{
    public async Task SendRequestAsync()
    {
        // Direct instantiation of HttpRequestOptions
        var requestOptions = new HttpRequestOptions();
        requestOptions.Request.Method = HttpMethod.Get;

        using var manager = new HttpManager();
        using var response = await manager.HttpAsync(
            new Uri("https://api.example.com/data"),
            o =>
            {
                o.Request.Method = HttpMethod.Get;
                o.Request.Headers.Add("Accept", "application/json");
            });
    }
}

```
