---
uid: Cuemon.AspNetCore.Diagnostics.ServerTiming
example:
- *content
---

The following example demonstrates how to use `ServerTiming` to communicate performance metrics via the Server-Timing header.

```csharp
using System;
using System.Linq;

        namespace Cuemon.AspNetCore.Diagnostics;

        public static class ServerTimingExample
        {
            public static void Demonstrate()
            {
                var timing = new ServerTiming();
        timing.AddServerTiming("db", TimeSpan.FromMilliseconds(12), "SQL query");

        var metric = timing.Metrics.First();
        Console.WriteLine($"{metric.Name}:{metric.Duration}");
            }
        }
```
