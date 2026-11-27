---
uid: Cuemon.AssignmentOperator
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AssignmentOperator"/> enum to specify arithmetic or bitwise compound assignment operations.

```csharp
// Program.cs
using System;
using Cuemon;

int total = Calculator.Calculate(5, AssignmentOperator.Addition, 3);
int shifted = Calculator.Calculate(5, AssignmentOperator.LeftShift, 2);

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Shifted: {shifted}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="921c240c8a0813ef4a27284b"><a href="~/run-actions/921c240c8a0813ef4a27284b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/921c240c8a0813ef4a27284b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
