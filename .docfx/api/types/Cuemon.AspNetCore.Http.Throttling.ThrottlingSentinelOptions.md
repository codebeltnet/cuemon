---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottlingSentinelOptions
example:
- *content
---

The following example shows how to create default and custom `ThrottlingSentinelOptions` to configure rate-limiting behavior. It demonstrates setting the quota, context resolver, header names, and retry-after scope, then validates the configuration and prints the selected values.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Http;

namespace MyApp.Http.Throttling
{
    public class ThrottlingSentinelOptionsExample
    {
        public ThrottlingSentinelOptions CreateDefault()
        {
            // Default: RateLimit-Limit header, 429 response with Retry-After
            var options = new ThrottlingSentinelOptions();
            return options;
        }

        public ThrottlingSentinelOptions CreateCustom()
        {
            var options = new ThrottlingSentinelOptions
            {
                // Allow 60 requests per 1 minute window per client
                Quota = new ThrottleQuota(60, TimeSpan.FromMinutes(1)),
                // Resolve context by client IP address
                ContextResolver = ctx =>
                    ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                // Custom header names
                RateLimitHeaderName = "X-Rate-Limit-Limit",
                RateLimitRemainingHeaderName = "X-Rate-Limit-Remaining",
                RateLimitResetHeaderName = "X-Rate-Limit-Reset",
                // Use delta-seconds for Retry-After
                RateLimitResetScope = RetryConditionScope.DeltaSeconds,
                UseRetryAfterHeader = true,
                RetryAfterScope = RetryConditionScope.DeltaSeconds,
                // Custom response message
                TooManyRequestsMessage = "Rate limit exceeded. Please slow down."
            };

            // Validate the configuration
            options.ValidateOptions();

            Console.WriteLine($"Quota: {options.Quota.RateLimit} req / {options.Quota.Window.TotalMinutes} min");
            Console.WriteLine($"RateLimitHeader: {options.RateLimitHeaderName}");
            Console.WriteLine($"RetryAfterScope: {options.RetryAfterScope}");

            return options;
        }
    }
}
```
