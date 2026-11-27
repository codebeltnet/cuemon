---
uid: Cuemon.ExceptionHandler`2
example:
- *content
---

The following example demonstrates how to use the generic <xref cref="Cuemon.ExceptionHandler{TException, TResult}"/> class in the fluent exception-triggering chain with an out-value (tester) condition.

```csharp
// Program.cs
using System;
using Cuemon;

TesterFunc<int, bool> parsePort = (out int value) => int.TryParse("70000", out value);

ExceptionHandler<ArgumentOutOfRangeException, int> handler =
    new ExceptionCondition<ArgumentOutOfRangeException>().IsTrue(parsePort);

var invoker = handler.Create(port =>
    new ArgumentOutOfRangeException(nameof(port), port, "Ports must be between 0 and 65535."));

try
{
    invoker.TryThrow();
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Rejected port: {ex.ActualValue}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a80ea690be127edb42abad6a"><a href="~/run-actions/a80ea690be127edb42abad6a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a80ea690be127edb42abad6a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
