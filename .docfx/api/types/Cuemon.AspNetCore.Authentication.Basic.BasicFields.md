---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicFields
example:
- *content
---

The following example demonstrates how to use the field constants of `BasicFields` to construct a Basic authorization header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Basic;

var builder = new BasicAuthorizationHeaderBuilder();
builder.AddUserName("alice");
builder.AddPassword("s3cret");
var header = builder.Build();

Console.WriteLine(BasicAuthorizationHeader.Scheme);
Console.WriteLine(BasicFields.Realm);
Console.WriteLine(BasicFields.Credentials);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="784b084d0280247b5d1f6a6f"><a href="~/run-actions/784b084d0280247b5d1f6a6f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/784b084d0280247b5d1f6a6f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
