---
uid: Cuemon.Extensions.AspNetCore.Hosting.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add the hosting environment HTTP header middleware to an ASP.NET Core application pipeline.

```csharp
using Cuemon.AspNetCore.Hosting;
using Cuemon.Extensions.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Examples;

public class StartupPipeline
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseHostingEnvironment(o =>
        {
            o.HeaderName = "X-Environment";
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

}
}

```
