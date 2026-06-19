---
uid: Cuemon.Diagnostics.Failure
example:
- *content
---

The following example demonstrates how to use the <see cref="Failure"/> record to expose structured exception details for diagnostics.

```csharp
using System;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public static class FailureExample
{
    public static void Demonstrate()
    {
        var exception = new InvalidOperationException("The operation could not be completed.");
        exception.Data["OperationId"] = "OP-42";

        var failure = new Failure(exception, FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data);

        Console.WriteLine(failure.Type);
        Console.WriteLine(failure.Message);
        Console.WriteLine(failure.Data["OperationId"]);
        Console.WriteLine(failure.GetUnderlyingSensitivity());
    }
}

```
