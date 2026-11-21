---
uid: Cuemon.ExceptionInvoker`1
example:
- *content
---

The following example demonstrates the shared workflow for the <see cref="ExceptionInvoker{TException}"/> family: evaluate a business condition, build the invoker from that condition, and call <c>TryThrow()</c> when the invalid state should surface as an exception.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;

var settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

ExceptionInvoker<InvalidOperationException> invoker =
    new ExceptionCondition<InvalidOperationException>()
        .IsTrue(() => !settings.ContainsKey("ConnectionString"))
        .Create(() => new InvalidOperationException("A ConnectionString setting is required before the data pipeline can start."));

bool threw = false;

try
{
    invoker.TryThrow();
}
catch (InvalidOperationException ex)
{
    threw = true;
    Console.WriteLine(ex.Message);
}

Console.WriteLine($"Threw: {threw}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7548b33e2c766577b35482b3"><a href="~/run-actions/7548b33e2c766577b35482b3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7548b33e2c766577b35482b3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
