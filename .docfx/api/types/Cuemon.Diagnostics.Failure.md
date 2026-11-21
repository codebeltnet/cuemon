---
uid: Cuemon.Diagnostics.Failure
example:
- *content
---

The following example demonstrates how to use the <see cref="Failure"/> record to expose structured exception details for diagnostics.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;

var exception = new InvalidOperationException("The operation could not be completed.");
exception.Data["OperationId"] = "OP-42";

var failure = new Failure(exception, FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data);

Console.WriteLine(failure.Type);
Console.WriteLine(failure.Message);
Console.WriteLine(failure.Data["OperationId"]);
Console.WriteLine(failure.GetUnderlyingSensitivity());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8a795658ae503c9d80e1a532"><a href="~/run-actions/8a795658ae503c9d80e1a532/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8a795658ae503c9d80e1a532/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
