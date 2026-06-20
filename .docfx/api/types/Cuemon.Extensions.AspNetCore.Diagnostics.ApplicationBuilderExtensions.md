---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add diagnostics middleware to the ASP.NET Core pipeline using the <xref:Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions> class.

```csharp
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;

namespace MyApp.Examples;

public class ApplicationBuilderExtensionsExample
{
    public void Configure(IApplicationBuilder app)
    {
        // Add Server-Timing header middleware
        app.UseServerTiming();

        // Add fault descriptor exception handler (catches exceptions and returns structured error responses)
        app.UseFaultDescriptorExceptionHandler();

}
}

```
