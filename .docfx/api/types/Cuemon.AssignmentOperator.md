---
uid: Cuemon.AssignmentOperator
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AssignmentOperator"/> enum to specify arithmetic or bitwise compound assignment operations.

```csharp
using System;
using Cuemon;

namespace Contoso.Billing;

public sealed class AssignmentOperatorExample
{
    public static void Run()
    {
        int total = Calculator.Calculate(5, AssignmentOperator.Addition, 3);
        int shifted = Calculator.Calculate(5, AssignmentOperator.LeftShift, 2);

        Console.WriteLine($"Total: {total}");
        Console.WriteLine($"Shifted: {shifted}");
    }
}
```
