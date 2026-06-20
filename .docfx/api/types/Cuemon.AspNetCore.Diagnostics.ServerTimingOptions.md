---
uid: Cuemon.AspNetCore.Diagnostics.ServerTimingOptions
example:
- *content
---

The following example demonstrates how to configure `ServerTimingOptions`.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;

namespace MyApp.Examples;

public class ServerTimingOptionsExample
{
    public void Demonstrate()
    {
        var options = new ServerTimingOptions
        {
            TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(10)
        };

}
}

```
