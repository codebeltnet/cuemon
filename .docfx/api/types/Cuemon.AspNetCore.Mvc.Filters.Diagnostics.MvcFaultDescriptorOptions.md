---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.MvcFaultDescriptorOptions
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.MvcFaultDescriptorOptions"/> for Problem Details responses and marks MVC exceptions as handled after the filter runs.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
namespace MyApp.Examples;

public static class MvcFaultDescriptorOptionsExample
{
    public static void Demonstrate()
    {
        var options = new MvcFaultDescriptorOptions
        {
            MarkExceptionHandled = true,
            FaultDescriptor = PreferredFaultDescriptor.ProblemDetails,
            SensitivityDetails = FaultSensitivityDetails.Failure
        };

        Console.WriteLine(options.MarkExceptionHandled);
        Console.WriteLine(options.FaultDescriptor);
        Console.WriteLine(options.HttpFaultResolvers.Count);
    }
}
```
