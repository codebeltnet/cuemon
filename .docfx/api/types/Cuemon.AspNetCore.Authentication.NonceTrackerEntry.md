---
uid: Cuemon.AspNetCore.Authentication.NonceTrackerEntry
example:
- *content
---

The following example demonstrates a nonce tracker entry that holds nonce data for authentication.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication;

var entry = new NonceTrackerEntry(1, DateTime.UtcNow);
Console.WriteLine($"Count: {entry.Count}, Created: {entry.Created}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3dbd1a9e97c17497277c18d3"><a href="~/run-actions/3dbd1a9e97c17497277c18d3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3dbd1a9e97c17497277c18d3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
