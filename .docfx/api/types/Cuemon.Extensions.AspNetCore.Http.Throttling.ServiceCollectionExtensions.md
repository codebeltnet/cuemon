---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register throttling and rate-limiting services in an ASP.NET Core application using ServiceCollectionExtensions, including in-memory throttling cache and custom rate-limit sentinel options.

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
