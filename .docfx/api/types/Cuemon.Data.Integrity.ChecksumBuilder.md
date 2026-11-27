---
uid: Cuemon.Data.Integrity.ChecksumBuilder
example:
- *content
---

The following example demonstrates how to use `ChecksumBuilder` to compute and compare checksums for arbitrary data.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Security;

var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
Console.WriteLine($"Empty checksum: {builder.Checksum.ToHexadecimalString()}");

builder.CombineWith(BitConverter.GetBytes(42L));
builder.CombineWith(Encoding.UTF8.GetBytes("Hello, World!"));

var comparison = new ChecksumBuilder(BitConverter.GetBytes(42L), () => HashFactory.CreateFnv128());
comparison.CombineWith(Encoding.UTF8.GetBytes("Hello, World!"));

Console.WriteLine($"Current checksum: {builder}");
Console.WriteLine($"Checksums match: {builder.Equals(comparison)}");
Console.WriteLine($"Hash code: {builder.GetHashCode()}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="af92af51de25fcdd014819bd"><a href="~/run-actions/af92af51de25fcdd014819bd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/af92af51de25fcdd014819bd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
