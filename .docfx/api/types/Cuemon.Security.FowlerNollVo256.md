---
uid: Cuemon.Security.FowlerNollVo256
example:
- *content
---

The following example demonstrates how to compute 256-bit FNV-1a hash values using FowlerNollVo256, with configurable byte order and integer conversion.

```csharp
// Program.cs
using System;
using System.Numerics;
using System.Text;
using Cuemon;
using Cuemon.Security;

// Create a FNV-1a 256-bit hash (default algorithm).
var fnv = new FowlerNollVo256();

Console.WriteLine($"Bits:        {fnv.Bits}");     // 256
Console.WriteLine($"Algorithm:   {fnv.Options.Algorithm}");  // Fnv1a

// Compute hash of a UTF-8 encoded string.
var data = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");
var hash = fnv.ComputeHash(data);

Console.WriteLine($"Hash (hex):  {hash.ToHexadecimalString()}");
Console.WriteLine($"Hash (b64):  {hash.ToBase64String()}");
Console.WriteLine($"Hash length: {hash.GetBytes().Length} bytes");

// Configure byte order independently.
var fnvLe = new FowlerNollVo256(o =>
{
    o.ByteOrder = Endianness.LittleEndian;
});
var hashLe = fnvLe.ComputeHash(data);
Console.WriteLine($"LE hash:     {hashLe.ToHexadecimalString()}");

// Convert hash to an integer using the built-in converter.
var asBigInt = hash.To(b => new BigInteger(b));
Console.WriteLine($"As integer:  {asBigInt}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7ef17850e7c84ccb8f84215f"><a href="~/run-actions/7ef17850e7c84ccb8f84215f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7ef17850e7c84ccb8f84215f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
