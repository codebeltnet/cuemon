---
uid: Cuemon.Extensions.Net.Http.SlimHttpClientFactoryOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactoryOptions"/> with a custom handler lifetime and use it alongside <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactory"/> to create named <see cref="System.Net.Http.HttpClient"/> instances.

```csharp
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;

namespace MyApp.Examples;

public class SlimHttpClientFactoryOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of SlimHttpClientFactoryOptions
        var factoryOptions = new SlimHttpClientFactoryOptions
        {
            HandlerLifetime = TimeSpan.FromSeconds(30)
        };

        var factory = new SlimHttpClientFactory(
            () => new HttpClientHandler(),
            o =>
            {
                o.HandlerLifetime = TimeSpan.FromSeconds(30);
            });

        // Create a named client
        using var client = factory.CreateClient("MyApi");
        client.BaseAddress = new Uri("https://api.example.com");

        Console.WriteLine($"Client ready: {client.BaseAddress}");

}
}

```
