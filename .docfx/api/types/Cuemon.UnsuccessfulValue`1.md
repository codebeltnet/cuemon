---
uid: Cuemon.UnsuccessfulValue`1
example:
- *content
---

The following example demonstrates how to use `UnsuccessfulValue<TResult>` to represent a typed operation that failed, carrying both the exception and a default result value.

```csharp
using System;
using Cuemon;

namespace Contoso.Calculations;

public sealed class UnsuccessfulValueOfTResultExample
{
    public static void Run()
    {
        ConditionalValue<int> outcome = Divide(10, 0);

        Console.WriteLine($"Succeeded: {outcome.Succeeded}");
        Console.WriteLine($"Result: {outcome.Result}");
        Console.WriteLine($"Failure: {outcome.Failure?.GetType().Name}");
    }

    private static ConditionalValue<int> Divide(int dividend, int divisor)
    {
        if (divisor == 0)
        {
            return new UnsuccessfulValue<int>(new DivideByZeroException("Cannot divide by zero."), -1);
        }

        return new SuccessfulValue<int>(dividend / divisor);
    }
}
```
