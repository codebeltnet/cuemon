---
uid: Cuemon.ExceptionInvoker`1
example:
- *content
---

The following example demonstrates the shared workflow for the <see cref="ExceptionInvoker{TException}"/> family: evaluate a business condition, build the invoker from that condition, and call <c>TryThrow()</c> when the invalid state should surface as an exception.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;

namespace MyApp.Examples;

public static class ExceptionInvokerExample
{
    public static void Demonstrate()
    {
        var settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        ExceptionInvoker<InvalidOperationException> invoker =
            new ExceptionCondition<InvalidOperationException>()
                .IsTrue(() => !settings.ContainsKey("ConnectionString"))
                .Create(() => new InvalidOperationException("A ConnectionString setting is required before the data pipeline can start."));

        bool threw = false;

        try
        {
            invoker.TryThrow();
        }
        catch (InvalidOperationException ex)
        {
            threw = true;
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine($"Threw: {threw}");
    }
}
```
