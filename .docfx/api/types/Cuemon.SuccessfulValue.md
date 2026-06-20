---
uid: Cuemon.SuccessfulValue
example:
- *content
---

The following example demonstrates how to use `SuccessfulValue` to represent a void operation that completed successfully, enabling consistent conditional-return patterns.

```csharp
using System;
using Cuemon;

namespace Contoso.Startup;

public sealed class SuccessfulValueExample
{
    public static void Run()
    {
        ConditionalValue outcome = WarmUpCache(dependenciesReady: true);

        Console.WriteLine($"Succeeded: {outcome.Succeeded}");
        Console.WriteLine($"Failure is null: {outcome.Failure is null}");
    }

    private static ConditionalValue WarmUpCache(bool dependenciesReady)
    {
        if (!dependenciesReady)
        {
            return new UnsuccessfulValue(new InvalidOperationException("Dependencies are missing."));
        }

        return new SuccessfulValue();
    }
}
```
