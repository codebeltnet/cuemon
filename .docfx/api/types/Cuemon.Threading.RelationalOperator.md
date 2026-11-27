---
uid: Cuemon.Threading.RelationalOperator
example:
- *content
---

The following example demonstrates how to use `RelationalOperator` with `ForLoopRuleset` and `AdvancedParallelFactory` to perform parallel work.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Threading;

var rules = new ForLoopRuleset<int>(0, 10, 2, RelationalOperator.LessThan);

AdvancedParallelFactory.For(rules, i =>
{
    Console.WriteLine($"Processing iteration: {i}");
});
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e40265e8d83a12a55c10245a"><a href="~/run-actions/e40265e8d83a12a55c10245a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e40265e8d83a12a55c10245a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
