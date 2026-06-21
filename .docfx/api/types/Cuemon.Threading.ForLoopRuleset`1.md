---
uid: Cuemon.Threading.ForLoopRuleset`1
example:
- *content
---

The following example demonstrates how to define a for-loop ruleset using ForLoopRuleset with configurable start, end, step, relational operator, and assignment operator.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Threading;

namespace Contoso.Scheduling;

public sealed class ForLoopRulesetExample
{
    public static void Run()
    {
        var rules = new ForLoopRuleset<int>(
            from: 0,
            to: 5,
            step: 1,
            relation: RelationalOperator.LessThanOrEqual,
            assignment: AssignmentOperator.Addition);

        var values = new List<int>();
        int current = rules.From;

        while (rules.Condition(current, rules.Relation, rules.To))
        {
            values.Add(current);
            current = rules.Iterator(current, rules.Assignment, rules.Step);
        }

        Console.WriteLine(string.Join(", ", values));
    }
}
```
