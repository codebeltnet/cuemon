---
uid: Cuemon.Security.FowlerNollVo128
example:
- *content
---

The following example demonstrates how to compute 128-bit FNV-1a and FNV-1 hash values using FowlerNollVo128, with configurable algorithm variant and byte order.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

// Create a FNV-1a 128-bit hash (default algorithm).
var fnv = new FowlerNollVo128();

Console.WriteLine($"Bits:        {fnv.Bits}");     // 128
Console.WriteLine($"Algorithm:   {fnv.Options.Algorithm}");  // Fnv1a

// Compute hash of a UTF-8 encoded string.
var data = Encoding.UTF8.GetBytes("Hello, World!");
var hash = fnv.ComputeHash(data);

Console.WriteLine($"Hash (hex):  {hash.ToHexadecimalString()}");
Console.WriteLine($"Hash (b64):  {hash.ToBase64String()}");

// Use FNV-1 instead of FNV-1a.
var fnv1 = new FowlerNollVo128(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
var hashFnv1 = fnv1.ComputeHash(data);
Console.WriteLine($"FNV-1 hash:  {hashFnv1}");

// Little-endian byte order.
var fnvLe = new FowlerNollVo128(o => o.ByteOrder = Endianness.LittleEndian);
var hashLe = fnvLe.ComputeHash(data);
Console.WriteLine($"LE hash:     {hashLe.ToHexadecimalString()}");

// Verify hash with the built-in offset basis for empty input.
var emptyHash = fnv.ComputeHash(Array.Empty<byte>());
Console.WriteLine($"Offset basis hash: {emptyHash.ToHexadecimalString()}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2efa38c4a9d4f4b0ee6ae79c"><a href="~/run-actions/2efa38c4a9d4f4b0ee6ae79c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2efa38c4a9d4f4b0ee6ae79c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
