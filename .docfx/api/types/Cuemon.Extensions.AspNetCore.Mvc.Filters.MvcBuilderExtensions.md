---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.MvcBuilderExtensions
example:
- *content
---

The following example configures the MVC builder with the same option families covered by the unit tests: API-key enforcement, throttling, user-agent validation, fault descriptors, and cache headers. It chains `AddApiKeySentinelOptions`, `AddThrottlingSentinelOptions`, `AddUserAgentSentinelOptions`, `AddFaultDescriptorOptions`, and `AddHttpCacheableOptions` on the MVC builder. The resulting service count is written to the console, demonstrating how to register security, throttling, and caching middleware in a single fluent configuration.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.DocExamples;

public sealed class MvcBuilderExtensionsExample
{
    public IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        var builder = services
            .AddMvc()
            .AddApiKeySentinelOptions(options =>
            {
                options.AllowedKeys.Add("demo-key");
                options.UseGenericResponse = true;
            })
            .AddThrottlingSentinelOptions(options =>
            {
                options.ContextResolver = context => context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
                options.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
                options.TooManyRequestsMessage = "Rate limit exceeded.";
            })
            .AddUserAgentSentinelOptions(options =>
            {
                options.RequireUserAgentHeader = true;
                options.ValidateUserAgentHeader = true;
                options.AllowedUserAgents.Add("DocsSample/1.0");
            })
            .AddFaultDescriptorOptions(options =>
            {
                options.MarkExceptionHandled = true;
                options.SensitivityDetails = FaultSensitivityDetails.All;
                options.HttpFaultResolvers.AddHttpFaultResolver<TooManyRequestsException>();
            })
            .AddHttpCacheableOptions(options =>
            {
                options.CacheControl.MaxAge = TimeSpan.FromMinutes(5);
                options.Filters.AddLastModifiedHeader();
                options.Filters.AddEntityTagHeader(entityTag =>
                {
                    entityTag.UseEntityTagResponseParser = true;
                });
            });

        Console.WriteLine(builder.Services.Count);
        return builder.Services;
    }
}
```
