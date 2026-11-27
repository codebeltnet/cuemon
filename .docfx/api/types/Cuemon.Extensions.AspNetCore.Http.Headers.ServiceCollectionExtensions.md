---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register API key and User-Agent validation rules and then inspect the resolved options through `IOptions<T>`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddOptions();
services.AddApiKeySentinelOptions(options =>
{
    options.AllowedKeys.Add("known-key");
    options.HeaderName = "X-Test-Key";
    options.UseGenericResponse = true;
});
services.AddUserAgentSentinelOptions(options =>
{
    options.AllowedUserAgents.Add("Cuemon-Agent");
    options.RequireUserAgentHeader = true;
    options.ValidateUserAgentHeader = true;
});

using var provider = services.BuildServiceProvider();
var apiKeyOptions = provider.GetRequiredService<IOptions<ApiKeySentinelOptions>>().Value;
var userAgentOptions = provider.GetRequiredService<IOptions<UserAgentSentinelOptions>>().Value;

Console.WriteLine($"{apiKeyOptions.HeaderName}:{apiKeyOptions.AllowedKeys.Count}");
Console.WriteLine($"{userAgentOptions.RequireUserAgentHeader}:{userAgentOptions.AllowedUserAgents.Count}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="67e4a45fc99112034de3a0b0"><a href="~/run-actions/67e4a45fc99112034de3a0b0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/67e4a45fc99112034de3a0b0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
