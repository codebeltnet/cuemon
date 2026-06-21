---
uid: Cuemon.Messaging.RequestToken
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Messaging.RequestToken"/> to uniquely identify an individual request within a system.

```csharp
using System;
using Cuemon.Messaging;

namespace MyApp.Examples;

public class RequestTokenExample
{
    public void Demonstrate()
    {
        // Each request gets its own unique ID
        var request1 = new RequestToken();
        var request2 = new RequestToken();

        Console.WriteLine($"Request 1 ID: {request1}");
        Console.WriteLine($"Request 2 ID: {request2}");
        Console.WriteLine($"Are they different? {request1.RequestId != request2.RequestId}");

}
}

```
