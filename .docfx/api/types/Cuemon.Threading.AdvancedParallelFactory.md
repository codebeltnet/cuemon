---
uid: Cuemon.Threading.AdvancedParallelFactory
example:
- *content
---

The following example demonstrates using `AdvancedParallelFactory` to compute an iterator value and evaluate a loop condition. The output shows the result of `5 + 3` and whether that result satisfies the greater-than-or-equal check of 8.

```csharp
using System;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class AdvancedParallelFactoryExample
{
    public void Demonstrate()
    {
        var next = AdvancedParallelFactory.Iterator(5, AssignmentOperator.Addition, 3);
        Console.WriteLine($"5 + 3 = {next}");

        var isComplete = AdvancedParallelFactory.Condition(next, RelationalOperator.GreaterThanOrEqual, 8);
        Console.WriteLine($"Is loop condition met? {isComplete}");
    }
}
```
