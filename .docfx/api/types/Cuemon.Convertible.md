---
uid: Cuemon.Convertible
example:
- *content
---

The following example shows how to convert common .NET types to byte arrays using `Convertible.GetBytes`. It demonstrates converting an integer and a string, then restoring the integer from its byte representation.

```csharp
// Program.cs
using System;
using Cuemon;

byte[] intBytes = Convertible.GetBytes(12345);
Console.WriteLine(intBytes.Length); // 4
Console.WriteLine(BitConverter.ToInt32(intBytes)); // 12345

byte[] stringBytes = Convertible.GetBytes("Hello");
Console.WriteLine(stringBytes.Length); // 5

int restored = BitConverter.ToInt32(intBytes);
Console.WriteLine(restored); // 12345
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f014e8b935d6c041436987ae"><a href="~/run-actions/f014e8b935d6c041436987ae/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f014e8b935d6c041436987ae/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
