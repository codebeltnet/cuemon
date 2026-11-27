---
uid: Cuemon.Security.FowlerNollVo32
example:
- *content
---

The following example demonstrates how to compute a 32-bit FNV-1a hash using the <see cref="FowlerNollVo32"/> class.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon; // for Endianness
using Cuemon.Security; // for FowlerNollVo32

var fnv = new FowlerNollVo32();

// Compute hash of a string
byte[] data = Encoding.UTF8.GetBytes("hello");
var hash = fnv.ComputeHash(data);

Console.WriteLine(hash.ToHexadecimalString()); // 4f9f2cab (big-endian default)
Console.WriteLine(hash.ToBase64String());      // T58sqw==
Console.WriteLine(fnv.Bits);                   // 32

// Configure for little-endian output
var fnvLe = new FowlerNollVo32(o => o.ByteOrder = Endianness.LittleEndian);
byte[] hashBytes = fnvLe.ComputeHash(data).GetBytes();
Console.WriteLine(BitConverter.ToString(hashBytes)); // AB-2C-9F-4F

// Use FNV-1 variant instead of FNV-1a
fnv.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
hash = fnv.ComputeHash(data);
Console.WriteLine(hash.ToHexadecimalString()); // b6fa7167
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bf4273330290999d91e70be8"><a href="~/run-actions/bf4273330290999d91e70be8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bf4273330290999d91e70be8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
