---
uid: Cuemon.Net.Http.HttpHeaderNames
example:
- *content
---

The following example demonstrates how to use <see cref="HttpHeaderNames"/> constants when working with HTTP request and response headers.

```csharp
// Program.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7d9d9f5c189b68b724135790"><a href="~/run-actions/7d9d9f5c189b68b724135790/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7d9d9f5c189b68b724135790/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
