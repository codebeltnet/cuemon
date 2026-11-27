---
uid: Cuemon.Security.FowlerNollVo64
example:
- *content
---

The following example demonstrates how to compute a 64-bit Fowler-Noll-Vo (FNV) hash using the <xref:Cuemon.Security.FowlerNollVo64> class.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security;
using Cuemon;

// Create FNV-1a 64-bit hash instance (default algorithm)
var fnv = new FowlerNollVo64();

// Compute hash from a string
byte[] data = Encoding.UTF8.GetBytes("hello");
HashResult hash = fnv.ComputeHash(data);

// Display the hash in hexadecimal format
Console.WriteLine(hash.ToHexadecimalString());

// Switch to FNV-1 algorithm
fnv.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
HashResult hashFnv1 = fnv.ComputeHash(data);
Console.WriteLine(hashFnv1.ToHexadecimalString());

// Change byte order to little endian
fnv.Options.ByteOrder = Endianness.LittleEndian;
HashResult hashLe = fnv.ComputeHash(data);
Console.WriteLine(hashLe.ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9fa573620262e8a8da61a76c"><a href="~/run-actions/9fa573620262e8a8da61a76c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9fa573620262e8a8da61a76c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
