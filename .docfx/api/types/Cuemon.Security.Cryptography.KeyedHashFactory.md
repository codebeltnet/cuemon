---
uid: Cuemon.Security.Cryptography.KeyedHashFactory
example:
- *content
---

The following example shows how to compute an HMAC-SHA256 hash for message authentication using `KeyedHashFactory`. It creates a keyed hash algorithm with a secret key, hashes an input string, and prints the hexadecimal digest.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var secret = Encoding.UTF8.GetBytes("my-secret-key");
var hmac = KeyedHashFactory.CreateHmacCryptoSha256(secret);
var input = "Message to authenticate";
var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
Console.WriteLine($"HMAC-SHA256: {BitConverter.ToString(hash.To<byte[]>(bytes => bytes)).Replace("-", "")}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1dd46290fd954e19986cbfdb"><a href="~/run-actions/1dd46290fd954e19986cbfdb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1dd46290fd954e19986cbfdb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
