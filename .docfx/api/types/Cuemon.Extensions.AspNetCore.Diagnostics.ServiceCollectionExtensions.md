---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.AspNetCore.Diagnostics.ServiceCollectionExtensions> extension methods to configure diagnostics and fault handling in an ASP.NET Core application.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace MyAspNetCoreApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add ServerTiming service for performance profiling
            services.AddServerTiming(options =>
            {
                options.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(10);
            });

            // Configure FaultDescriptor options (exception handling)
            services.AddFaultDescriptorOptions(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.All;
                options.RootHelpLink = new Uri("https://example.com/help");
            });

            // Configure ExceptionDescriptor options
            services.AddExceptionDescriptorOptions(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.None;
            });

            // Configure ServerTiming options separately
            services.AddServerTimingOptions(options =>
            {
                options.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(50);
            });

            // Post-configure all IExceptionDescriptorOptions instances
            services.PostConfigureAllExceptionDescriptorOptions(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace;
            });

}}
}

```
