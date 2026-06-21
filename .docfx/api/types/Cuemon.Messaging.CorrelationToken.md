---
uid: Cuemon.Messaging.CorrelationToken
example:
- *content
---

The following example shows how to create one correlation identifier and pass it through related work.

```csharp
using System;
using Cuemon.Messaging;

namespace MyApp.Examples;

public static class CorrelationTokenExample
{
    public static void Demonstrate()
    {
        var generated = new CorrelationToken();
        var provided = new CorrelationToken("order-2026-0001");

        Console.WriteLine(generated.CorrelationId.Length == 32);
        Console.WriteLine(provided.ToString());
        Console.WriteLine(AttachToMessage("InventoryReserved", provided));
    }

    private static string AttachToMessage(string messageType, ICorrelationToken token)
    {
        return $"{messageType}:{token.CorrelationId}";
    }
}
```
