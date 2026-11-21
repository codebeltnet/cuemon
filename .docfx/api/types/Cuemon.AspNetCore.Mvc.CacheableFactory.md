---
uid: Cuemon.AspNetCore.Mvc.CacheableFactory
example:
- *content
---

The following example demonstrates how to create cacheable response objects using `CacheableFactory`.

```csharp
// Program.cs
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

var content = "hello-world";

var lastModified = CacheableFactory.CreateHttpLastModified(content, o =>
{
    o.TimestampProvider = _ => DateTime.UtcNow;
    o.ChangedTimestampProvider = _ => DateTime.UtcNow;
});

var entityTag = CacheableFactory.CreateHttpEntityTag(content, o =>
{
    o.ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value));
    o.WeakChecksumProvider = _ => false;
});

var combined = CacheableFactory.Create(content, o =>
{
    o.TimestampProvider = _ => DateTime.UtcNow;
    o.ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value));
    o.ChangedTimestampProvider = _ => DateTime.UtcNow;
    o.WeakChecksumProvider = _ => false;
});

Console.WriteLine(lastModified is ICacheableObjectResult);
Console.WriteLine(entityTag is ICacheableObjectResult);
Console.WriteLine(combined is ICacheableObjectResult);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a9c21ea8954b3fe52501f453"><a href="~/run-actions/a9c21ea8954b3fe52501f453/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a9c21ea8954b3fe52501f453/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
