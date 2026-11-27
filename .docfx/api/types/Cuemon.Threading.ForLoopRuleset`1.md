---
uid: Cuemon.Threading.ForLoopRuleset`1
example:
- *content
---

The following example demonstrates how to define a for-loop ruleset using ForLoopRuleset with configurable start, end, step, relational operator, and assignment operator.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Threading;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d9fa2d65af955fe21a4294a7"><a href="~/run-actions/d9fa2d65af955fe21a4294a7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d9fa2d65af955fe21a4294a7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
