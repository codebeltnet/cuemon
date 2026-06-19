---
uid: Cuemon.ExceptionInsights
example:
- *content
---

```csharp
using System;
using System.Reflection;

namespace Cuemon;

public class ExceptionInsightsExample
{
    public void Demonstrate()
    {
        try
        {
            throw new InvalidOperationException("Something went wrong.");
        }
        catch (InvalidOperationException ex)
        {
            // Enrich the exception with thread and environment information
            ExceptionInsights.Embed(ex,
                runtimeParameters: new object[] { "param1", 42 },
                snapshots: SystemSnapshots.CaptureThreadInfo | SystemSnapshots.CaptureEnvironmentInfo);

            // The enriched exception now has embedded insight data in its Data dictionary
            Console.WriteLine(ex.Data.Contains(ExceptionInsights.Key)
                ? "Insights embedded successfully."
                : "No insights available.");
        }
    }
}
```
