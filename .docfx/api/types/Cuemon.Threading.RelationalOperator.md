---
uid: Cuemon.Threading.RelationalOperator
example:
- *content
---

The following example demonstrates how to use `RelationalOperator` with `ForLoopRuleset` and `AdvancedParallelFactory` to perform parallel work.

```csharp
using System;
using Cuemon;
using Cuemon.Threading;

namespace MyApp.Examples;

public class RelationalOperatorExample
{
    public void Demonstrate()
    {
        var rules = new ForLoopRuleset<int>(0, 10, 2, RelationalOperator.LessThan);

        AdvancedParallelFactory.For(rules, i =>
        {
            Console.WriteLine($"Processing iteration: {i}");
        });

}
}

```
