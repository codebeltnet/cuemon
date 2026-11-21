---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader
example:
- *content
---

The following example demonstrates how to serialize and parse a Basic authorization header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Basic;

var header = new BasicAuthorizationHeader("Agent", "Test");
var parsed = BasicAuthorizationHeader.Create(header.ToString());

Console.WriteLine(parsed.UserName);
Console.WriteLine(parsed.Password);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3b5f060e17513c9295871392"><a href="~/run-actions/3b5f060e17513c9295871392/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3b5f060e17513c9295871392/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
