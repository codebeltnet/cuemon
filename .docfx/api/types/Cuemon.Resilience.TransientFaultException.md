---
uid: Cuemon.Resilience.TransientFaultException
example:
- *content
---

The following example demonstrates how to construct a <see cref="TransientFaultException" /> with retry evidence.

```csharp
using System;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Resilience;

namespace MyApp.Examples;

public static class TransientFaultExceptionExample
{
    public static void Demonstrate()
    {
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
    }
}
```
