---
uid: Cuemon.Resilience.TransientFaultException
example:
- *content
---

The following example demonstrates how to construct a <see cref="TransientFaultException" /> with retry evidence.

```csharp
// Program.cs
using System;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Resilience;

var evidence = new TransientFaultEvidence(
    attempts: 5,
    recoveryWaitTime: TimeSpan.FromSeconds(2),
    totalRecoveryWaitTime: TimeSpan.FromSeconds(10),
    latency: TimeSpan.FromSeconds(1),
    descriptor: MethodDescriptor.Create(MethodBase.GetCurrentMethod()!));

var exception = new TransientFaultException("Operation failed after retries.", evidence);

Console.WriteLine(exception.Message);
Console.WriteLine(exception.Evidence.Attempts);
Console.WriteLine(exception.Evidence.TotalRecoveryWaitTime);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e160e2f577d47e1ce4f2d98d"><a href="~/run-actions/e160e2f577d47e1ce4f2d98d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e160e2f577d47e1ce4f2d98d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
