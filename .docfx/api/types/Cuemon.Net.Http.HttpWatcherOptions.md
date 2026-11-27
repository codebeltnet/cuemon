---
uid: Cuemon.Net.Http.HttpWatcherOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="HttpWatcherOptions" /> before creating an <see cref="HttpWatcher" />.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Net.Http;
using Cuemon.Security;

var options = new HttpWatcherOptions
{
    ClientFactory = () => new HttpClient(new HttpClientHandler(), false),
    HashFactory = () => new CyclicRedundancyCheck64(),
    ReadResponseBody = true,
    Period = TimeSpan.FromSeconds(5)
};

options.ValidateOptions();

Console.WriteLine(options.ReadResponseBody);
Console.WriteLine(options.Period.TotalSeconds);
Console.WriteLine($"HTTP client factory configured: {options.ClientFactory is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="541c2c4d3bdaff9727d5bc18"><a href="~/run-actions/541c2c4d3bdaff9727d5bc18/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/541c2c4d3bdaff9727d5bc18/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
