---
uid: Cuemon.Reflection.MethodSignature
example:
- *content
---

The following example shows how to capture lightweight method metadata for logging or retry evidence.

```csharp
using System;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class MethodSignatureExample
{
    public static void Demonstrate()
    {
        var signature = new MethodSignature(
            typeof(PaymentGateway).FullName ?? nameof(PaymentGateway),
            nameof(PaymentGateway.Authorize),
            new[] { typeof(string).Name, typeof(decimal).Name },
            new object[] { "INV-42", 19.95m });

        Console.WriteLine(signature.ToString());
        Console.WriteLine(string.Join(", ", signature.Parameters ?? Array.Empty<string>()));
        Console.WriteLine(signature.Arguments?.Length ?? 0);
    }
}

public sealed class PaymentGateway
{
    public void Authorize(string orderId, decimal amount)
    {
    }
}
```
