---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ServiceCollectionExtensions
example:
- *content
---

`ServiceCollectionExtensions` in the `Throttling` namespace registers rate-limiting services in an ASP.NET Core `IServiceCollection`. This example calls `AddMemoryThrottlingCache` and `AddThrottlingCache<MemoryThrottlingCache>` to register in-memory and custom throttling cache implementations as singletons, then configures `ThrottlingSentinelOptions` with a `ContextResolver` that uses the remote IP address, a quota of `100` requests per minute, custom rate-limit header names (`X-RateLimit-Limit`, `X-RateLimit-Remaining`, `X-RateLimit-Reset`), and `RetryAfter` header behavior in delta-seconds format. After this setup in `ConfigureServices`, the middleware pipeline can enforce the configured throttling rules for each client.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddMemoryThrottlingCache();
services.AddThrottlingCache<MemoryThrottlingCache>();
services.AddThrottlingSentinelOptions(options =>
{
    options.ContextResolver = context => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    options.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
    options.TooManyRequestsMessage = "API rate limit exceeded. Please wait before retrying.";
    options.RateLimitHeaderName = "X-RateLimit-Limit";
    options.RateLimitRemainingHeaderName = "X-RateLimit-Remaining";
    options.RateLimitResetHeaderName = "X-RateLimit-Reset";
    options.RateLimitResetScope = RetryConditionScope.DeltaSeconds;
    options.UseRetryAfterHeader = true;
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<ThrottlingSentinelOptions>>().Value;

Console.WriteLine(options.RateLimitHeaderName);
Console.WriteLine(options.Quota != null);
Console.WriteLine(options.TooManyRequestsMessage);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0d7b317eceacd3bf2c673126"><a href="~/run-actions/0d7b317eceacd3bf2c673126/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0d7b317eceacd3bf2c673126/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
