---
uid: Cuemon.Extensions.Net.Http.SlimHttpClientFactory
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactory"/> to create named <see cref="System.Net.Http.HttpClient"/> instances with a shared handler pool and configurable lifetime.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7846bb1a7af981bd6eb55464"><a href="~/run-actions/7846bb1a7af981bd6eb55464/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7846bb1a7af981bd6eb55464/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
