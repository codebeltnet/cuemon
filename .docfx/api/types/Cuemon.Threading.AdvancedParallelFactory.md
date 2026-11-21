---
uid: Cuemon.Threading.AdvancedParallelFactory
example:
- *content
---

The following example demonstrates using `AdvancedParallelFactory` to compute an iterator value and evaluate a loop condition. The output shows the result of `5 + 3` and whether that result satisfies the greater-than-or-equal check of 8.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Threading;

var next = AdvancedParallelFactory.Iterator(5, AssignmentOperator.Addition, 3);
Console.WriteLine($"5 + 3 = {next}");

var isComplete = AdvancedParallelFactory.Condition(next, RelationalOperator.GreaterThanOrEqual, 8);
Console.WriteLine($"Is loop condition met? {isComplete}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="378f2cb9e4e9b97a25319b50"><a href="~/run-actions/378f2cb9e4e9b97a25319b50/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/378f2cb9e4e9b97a25319b50/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
