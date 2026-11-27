---
uid: Cuemon.AspNetCore.Mvc.ContentBasedObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.ContentBasedObjectResultOptions{T}"/> to provide a checksum provider for generating ETags.

```csharp
// Program.cs
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

var options = new ContentBasedObjectResultOptions<string>
{
    ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value)),
    WeakChecksumProvider = _ => false // use strong ETag
};

// Validate that the required properties are configured
options.ValidateOptions();

Console.WriteLine("ChecksumProvider is configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8eb48fab6caa4c4c83e5a38a"><a href="~/run-actions/8eb48fab6caa4c4c83e5a38a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8eb48fab6caa4c4c83e5a38a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
