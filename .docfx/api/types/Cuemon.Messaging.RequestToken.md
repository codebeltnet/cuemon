---
uid: Cuemon.Messaging.RequestToken
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Messaging.RequestToken"/> to uniquely identify an individual request within a system.

```csharp
// Program.cs
using System;
using Cuemon.Messaging;

// Each request gets its own unique ID
var request1 = new RequestToken();
var request2 = new RequestToken();

Console.WriteLine($"Request 1 ID: {request1}");
Console.WriteLine($"Request 2 ID: {request2}");
Console.WriteLine($"Are they different? {request1.RequestId != request2.RequestId}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fdafd0473d3d2579eb4f8099"><a href="~/run-actions/fdafd0473d3d2579eb4f8099/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fdafd0473d3d2579eb4f8099/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
