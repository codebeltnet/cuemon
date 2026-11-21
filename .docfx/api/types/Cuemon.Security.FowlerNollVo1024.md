---
uid: Cuemon.Security.FowlerNollVo1024
example:
- *content
---

The following example shows how to compute 1024-bit Fowler-Noll-Vo hashes for the same payload with different algorithms.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

var hasher = new FowlerNollVo1024(options =>
{
    options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
    options.ByteOrder = Endianness.BigEndian;
});

HashResult orderHash = hasher.ComputeHash(Encoding.UTF8.GetBytes("order-42"));

Console.WriteLine(hasher.Bits);
Console.WriteLine(orderHash.GetBytes().Length);
Console.WriteLine(orderHash.ToHexadecimalString());

var legacyHasher = new FowlerNollVo1024(options => options.Algorithm = FowlerNollVoAlgorithm.Fnv1);
Console.WriteLine(legacyHasher.ComputeHash("order-42").ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="51ca3e552f55134e9006c7e7"><a href="~/run-actions/51ca3e552f55134e9006c7e7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/51ca3e552f55134e9006c7e7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
