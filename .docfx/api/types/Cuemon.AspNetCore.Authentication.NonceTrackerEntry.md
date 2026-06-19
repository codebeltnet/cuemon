---
uid: Cuemon.AspNetCore.Authentication.NonceTrackerEntry
example:
- *content
---

The following example demonstrates a nonce tracker entry that holds nonce data for authentication.

```csharp
using System;
using Cuemon.AspNetCore.Authentication;

namespace MyApp.Examples;

public class NonceTrackerEntryExample
{
    public void Demonstrate()
    {
        var entry = new NonceTrackerEntry(1, DateTime.UtcNow);
        Console.WriteLine($"Count: {entry.Count}, Created: {entry.Created}");
    }
}

```
