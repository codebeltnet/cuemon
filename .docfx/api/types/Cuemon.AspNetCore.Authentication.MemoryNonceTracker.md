---
uid: Cuemon.AspNetCore.Authentication.MemoryNonceTracker
example:
- *content
---

The following example demonstrates how to add, inspect, and remove nonce entries from the in-memory tracker.

```csharp
using System;
using Cuemon.AspNetCore.Authentication;

namespace MyApp.Examples;

public static class MemoryNonceTrackerExample
{
    public static void Demonstrate()
    {
        using var tracker = new MemoryNonceTracker();

        tracker.TryAddEntry("nonce-1", 7);

        if (tracker.TryGetEntry("nonce-1", out var entry))
        {
            Console.WriteLine(entry.Count);
        }

        Console.WriteLine(tracker.TryRemoveEntry("nonce-1"));
    }
}

```
