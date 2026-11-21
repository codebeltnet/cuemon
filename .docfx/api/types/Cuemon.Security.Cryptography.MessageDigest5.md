---
uid: Cuemon.Security.Cryptography.MessageDigest5
example:
- *content
---

The following example demonstrates how to compute an MD5 hash with <see cref="MessageDigest5" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new MessageDigest5();
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));

Console.WriteLine(MessageDigest5.BitSize);
Console.WriteLine(result.GetBytes().Length);
Console.WriteLine(result.ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f4f1da0ff1a479d0c033197f"><a href="~/run-actions/f4f1da0ff1a479d0c033197f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f4f1da0ff1a479d0c033197f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
