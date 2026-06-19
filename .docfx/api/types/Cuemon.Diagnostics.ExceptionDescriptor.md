---
uid: Cuemon.Diagnostics.ExceptionDescriptor
example:
- *content
---

The following example demonstrates how to create <see cref="ExceptionDescriptor"/> and enrich it with contextual evidence.

```csharp
using System;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public static class ExceptionDescriptorExample
{
    public static void Demonstrate()
    {
        var descriptor = new ExceptionDescriptor(
            new InvalidOperationException("Order 12345 has already been processed."),
            code: "OrderAlreadyProcessed",
            message: "The order cannot be modified because it has already been processed.",
            helpLink: new Uri("https://docs.example.com/errors/order-already-processed"));

        descriptor.AddEvidence("OrderId", 12345, value => value);
        descriptor.AddEvidence("UserId", "alice@example.com", value => value);

        Console.WriteLine(descriptor.Code);
        Console.WriteLine(descriptor.Message);
        Console.WriteLine(descriptor.Evidence.Count);
    }
}

```
