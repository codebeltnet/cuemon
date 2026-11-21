---
uid: Cuemon.Text.Stem
example:
- *content
---

The following example demonstrates how to use `Stem` to build a URL path by attaching prefixes and suffixes without duplication.

```csharp
// Program.cs
using System;
using Cuemon.Text;

var path = new Stem("api")
    .AttachPrefix("/")
    .AttachSuffix("/")
    .AttachSuffix("v1")
    .AttachSuffix("/")
    .AttachSuffix("users");

var result = path.ToString();
// result == "/api/v1/users"
Console.WriteLine(result);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6bb00540d68803f2dd399d85"><a href="~/run-actions/6bb00540d68803f2dd399d85/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6bb00540d68803f2dd399d85/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
