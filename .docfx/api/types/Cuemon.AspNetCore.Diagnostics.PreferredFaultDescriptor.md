---
uid: Cuemon.AspNetCore.Diagnostics.PreferredFaultDescriptor
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Diagnostics.PreferredFaultDescriptor"/> enum to configure the error response format for the `FaultDescriptorFilter`.

```csharp
using System;

namespace Cuemon.AspNetCore.Diagnostics;

public static class PreferredFaultDescriptorExample
{
    public static void Demonstrate()
    {
        var preferredFormat = PreferredFaultDescriptor.ProblemDetails;
Console.WriteLine(preferredFormat);
    }
}
```
