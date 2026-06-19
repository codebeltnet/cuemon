---
uid: Cuemon.Extensions.Net.Http.SlimHttpClientFactory
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactory"/> to create named <see cref="System.Net.Http.HttpClient"/> instances with a shared handler pool and configurable lifetime.

```csharp
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;

namespace MyApp.Examples;

public class SlimHttpClientFactoryExample
{
    public void Demonstrate()
    {
        // Create a factory that reuses HttpClientHandler instances per name
        var factory = new SlimHttpClientFactory(
            () => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                MaxAutomaticRedirections = 5
            });

        // Create two named clients - they share the same handler pool
        using var clientA = factory.CreateClient("ServiceA");
        using var clientB = factory.CreateClient("ServiceB");

        clientA.BaseAddress = new Uri("https://service-a.example.com");
        clientB.BaseAddress = new Uri("https://service-b.example.com");

        Console.WriteLine($"ClientA base: {clientA.BaseAddress}");
        Console.WriteLine($"ClientB base: {clientB.BaseAddress}");

        // The handler for "ServiceA" is reused across calls to CreateClient("ServiceA")
        // until the configured handler lifetime expires.

}
}

```
