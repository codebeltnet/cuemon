---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter
example:
- *content
---

The following example creates <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter"/> directly from configured packet-local options.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class FaultDescriptorFilterExample
{
    public static void Demonstrate()
    {
        var options = Options.Create(new MvcFaultDescriptorOptions
        {
            MarkExceptionHandled = true,
            FaultDescriptor = PreferredFaultDescriptor.ProblemDetails,
            SensitivityDetails = FaultSensitivityDetails.Failure
        });

        var filter = new FaultDescriptorFilter(options);

        Console.WriteLine(filter.Options.MarkExceptionHandled);
        Console.WriteLine(filter.Options.FaultDescriptor);
    }
}
```
