---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build a Basic Authorization header value using `BasicAuthorizationHeaderBuilder`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Basic;

var builder = new BasicAuthorizationHeaderBuilder();
builder.AddUserName("alice");
builder.AddPassword("password");
var headerValue = builder.Build(); // "Basic YWxpY2U6cGFzc3dvcmQ="
Console.WriteLine(headerValue);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9a75ce2826c1578cd66ba2f7"><a href="~/run-actions/9a75ce2826c1578cd66ba2f7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9a75ce2826c1578cd66ba2f7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
