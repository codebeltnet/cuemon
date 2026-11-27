---
uid: Cuemon.Security.Cryptography.UnkeyedHashFactory
example:
- *content
---

The following example demonstrates how to compute SHA-256 and SHA-512 hashes for data integrity using `UnkeyedHashFactory`. It hashes a sample input and prints the hexadecimal digest for each algorithm.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var sha256 = UnkeyedHashFactory.CreateCryptoSha256();
var input = "Data to hash";
var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
Console.WriteLine($"SHA-256: {BitConverter.ToString(hash.GetBytes()).Replace("-", "")}");

var sha512 = UnkeyedHashFactory.CreateCryptoSha512();
hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
Console.WriteLine($"SHA-512: {BitConverter.ToString(hash.GetBytes()).Replace("-", "")}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3241bf02f377dc7283d5e00c"><a href="~/run-actions/3241bf02f377dc7283d5e00c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3241bf02f377dc7283d5e00c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
