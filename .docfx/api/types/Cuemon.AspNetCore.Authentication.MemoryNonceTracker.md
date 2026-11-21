---
uid: Cuemon.AspNetCore.Authentication.MemoryNonceTracker
example:
- *content
---

The following example demonstrates how to add, inspect, and remove nonce entries from the in-memory tracker.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication;

using var tracker = new MemoryNonceTracker();

tracker.TryAddEntry("nonce-1", 7);

if (tracker.TryGetEntry("nonce-1", out var entry))
{
    Console.WriteLine(entry.Count);
}

Console.WriteLine(tracker.TryRemoveEntry("nonce-1"));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ca0b6187b9f0b2f6ec1235d2"><a href="~/run-actions/ca0b6187b9f0b2f6ec1235d2/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ca0b6187b9f0b2f6ec1235d2/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
