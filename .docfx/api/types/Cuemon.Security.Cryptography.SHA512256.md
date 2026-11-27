---
uid: Cuemon.Security.Cryptography.SHA512256
example:
- *content
---

The following example demonstrates how to use the low-level <see cref="SHA512256" /> implementation directly through <see cref="System.Security.Cryptography.HashAlgorithm" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

using var algorithm = new SHA512256();

var digest = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Hello, world!"));
algorithm.Initialize();
var secondDigest = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Hello, world!"));

Console.WriteLine(digest.Length);
Console.WriteLine(digest.Length == secondDigest.Length);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="675e91b79ea3b606b7055877"><a href="~/run-actions/675e91b79ea3b606b7055877/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/675e91b79ea3b606b7055877/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
