---
uid: Cuemon.AspNetCore.Http.HttpRequestDecoratorExtensions
example:
- *content
---

The following example demonstrates how to inspect HTTP request state using the <xref:Cuemon.AspNetCore.Http.HttpRequestDecoratorExtensions> class accessed through the <xref:Cuemon.Decorator> class.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Cuemon.AspNetCore.Http;

var context = new DefaultHttpContext();
context.Request.Method = HttpMethods.Get;

var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
context.Request.Headers[HeaderNames.IfNoneMatch] =
    string.Concat("\"", builder.Checksum.ToHexadecimalString(), "\"");

var request = Decorator.Enclose(context.Request);
var canServeFromCache = request.IsGetOrHeadMethod() && request.IsClientSideResourceCached(builder);

Console.WriteLine(canServeFromCache);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a116b0c2e5cef9d9afb997a3"><a href="~/run-actions/a116b0c2e5cef9d9afb997a3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a116b0c2e5cef9d9afb997a3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
