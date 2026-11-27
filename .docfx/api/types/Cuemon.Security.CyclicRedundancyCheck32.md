---
uid: Cuemon.Security.CyclicRedundancyCheck32
example:
- *content
---

The following example shows how to compute a CRC-32 checksum with the common reflected configuration.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

var checksum = new CyclicRedundancyCheck32(setup: options =>
{
    options.ByteOrder = Endianness.BigEndian;
    options.ReflectInput = true;
    options.ReflectOutput = true;
});

HashResult result = checksum.ComputeHash(Encoding.ASCII.GetBytes("123456789"));

Console.WriteLine(result.ToHexadecimalString().ToLowerInvariant());
Console.WriteLine(checksum.InitialValue);
Console.WriteLine(checksum.FinalXor);

var alternate = new CyclicRedundancyCheck32(polynomial: 0xEDB88320, initialValue: 0xFFFFFFFF, finalXor: 0xFFFFFFFF);
Console.WriteLine(alternate.ComputeHash("Cuemon").ToBase64String());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="20f32bdaa2bfef37a9946193"><a href="~/run-actions/20f32bdaa2bfef37a9946193/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/20f32bdaa2bfef37a9946193/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
