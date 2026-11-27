---
uid: Cuemon.Extensions.Net.Http.SlimHttpClientFactoryOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactoryOptions"/> with a custom handler lifetime and use it alongside <see cref="Cuemon.Extensions.Net.Http.SlimHttpClientFactory"/> to create named <see cref="System.Net.Http.HttpClient"/> instances.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;

// Direct instantiation of SlimHttpClientFactoryOptions
var factoryOptions = new SlimHttpClientFactoryOptions
{
    HandlerLifetime = TimeSpan.FromSeconds(30)
};

var factory = new SlimHttpClientFactory(
    () => new HttpClientHandler(),
    o =>
    {
        o.HandlerLifetime = factoryOptions.HandlerLifetime;
    });

// Create a named client
using var client = factory.CreateClient("MyApi");
client.BaseAddress = new Uri("https://api.example.com");

Console.WriteLine($"HandlerLifetime: {factoryOptions.HandlerLifetime.TotalSeconds}s, Client ready: {client.BaseAddress}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1dd14cdefec836e9307570e7"><a href="~/run-actions/1dd14cdefec836e9307570e7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1dd14cdefec836e9307570e7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
