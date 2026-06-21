---
uid: Cuemon.ExceptionHandler`2
example:
- *content
---

The following example demonstrates how to use the generic <xref cref="Cuemon.ExceptionHandler{TException, TResult}"/> class in the fluent exception-triggering chain with an out-value (tester) condition.

```csharp
using System;
using Cuemon;

namespace Cuemon.DocfxExamples;

public sealed class ExceptionHandlerOfTResultExample
{
    public static void Run()
    {
        TesterFunc<int, bool> parsePort = (out int value) => int.TryParse("70000", out value);

        ExceptionHandler<ArgumentOutOfRangeException, int> handler =
            new ExceptionCondition<ArgumentOutOfRangeException>().IsTrue(parsePort);

        var invoker = handler.Create(port =>
            new ArgumentOutOfRangeException(nameof(port), port, "Ports must be between 0 and 65535."));

        try
        {
            invoker.TryThrow();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Rejected port: {ex.ActualValue}");
        }
    }
}
```
