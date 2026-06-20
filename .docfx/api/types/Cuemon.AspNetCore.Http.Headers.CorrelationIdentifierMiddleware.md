---
uid: Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierMiddleware
example:
- *content
---

The following example demonstrates how to register `CorrelationIdentifierMiddleware` in the ASP.NET Core pipeline with a custom header name and correlation token. It then reads the correlation ID from an endpoint to confirm the middleware is working.

```csharp
using System.Threading.Tasks;
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Messaging;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Http.Headers
{
    public class CorrelationIdentifierMiddlewareRegistration
    {
        // Called from Startup.ConfigureServices or Program.cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CorrelationIdentifierOptions>(o =>
            {
                // Use a custom header name
                o.HeaderName = HttpHeaderNames.XCorrelationId;
                // Provide a specific correlation token
                o.Token = new CorrelationToken();
            });
        }

        // Called from Startup.Configure or Program.cs
        public void Configure(IApplicationBuilder app)
        {
            // Add the Correlation ID middleware to the pipeline
            app.UseMiddleware<CorrelationIdentifierMiddleware>();

            // Example endpoint that reads the correlation ID
            app.Run(async context =>
            {
                var correlationId = context.Items[CorrelationIdentifierMiddleware.HttpContextItemsKey];
                await context.Response.WriteAsync(
                    $"Correlation ID: {correlationId}");
            });
        }
    }
}

```
