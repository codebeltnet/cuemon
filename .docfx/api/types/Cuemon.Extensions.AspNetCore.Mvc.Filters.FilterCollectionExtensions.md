---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.FilterCollectionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.AspNetCore.Mvc.Filters.FilterCollectionExtensions> extension methods to register common ASP.NET Core MVC filters.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MyAspNetCoreApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                // Add HTTP cacheable filter for response caching
                options.Filters.AddHttpCacheable();

                // Add developer-friendly fault descriptor
                options.Filters.AddFaultDescriptor();

                // Add server timing header for performance profiling
                options.Filters.AddServerTiming();

                // Add User-Agent sentinel filter
                options.Filters.AddUserAgentSentinel();

                // Add API throttling sentinel
                options.Filters.AddThrottlingSentinel();

                // Add API key sentinel
                options.Filters.AddApiKeySentinel();
            });

}}
}

```
