---
uid: Cuemon.Threading.AdvancedParallelFactory
example:
- *content
---

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
