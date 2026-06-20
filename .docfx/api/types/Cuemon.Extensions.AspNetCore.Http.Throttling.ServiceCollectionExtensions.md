---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ServiceCollectionExtensions
example:
- *content
---

`ServiceCollectionExtensions` in the `Throttling` namespace registers rate-limiting services in an ASP.NET Core `IServiceCollection`. This example calls `AddMemoryThrottlingCache` and `AddThrottlingCache<MemoryThrottlingCache>` to register in-memory and custom throttling cache implementations as singletons, then configures `ThrottlingSentinelOptions` with a `ContextResolver` that uses the remote IP address, a quota of `100` requests per minute, custom rate-limit header names (`X-RateLimit-Limit`, `X-RateLimit-Remaining`, `X-RateLimit-Reset`), and `RetryAfter` header behavior in delta-seconds format. After this setup in `ConfigureServices`, the middleware pipeline can enforce the configured throttling rules for each client.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.AspNetCore.Throttling
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the in-memory throttling cache as a singleton
            services.AddMemoryThrottlingCache();

            // Register a custom IThrottlingCache implementation
            services.AddThrottlingCache<MemoryThrottlingCache>();

            // Configure throttling sentinel options (rate limiting rules)
            services.AddThrottlingSentinelOptions(o =>
            {
                // Identify clients by their remote IP address
                o.ContextResolver = ctx => ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                // Allow 100 requests per minute per client
                o.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);

                // Customize the rate limit response message
                o.TooManyRequestsMessage = "API rate limit exceeded. Please wait before retrying.";

                // Customize HTTP header names
                o.RateLimitHeaderName = "X-RateLimit-Limit";
                o.RateLimitRemainingHeaderName = "X-RateLimit-Remaining";
                o.RateLimitResetHeaderName = "X-RateLimit-Reset";

                // Use delta-seconds format for reset headers
                o.RateLimitResetScope = RetryConditionScope.DeltaSeconds;
                o.UseRetryAfterHeader = true;
            });

}}
}

```
