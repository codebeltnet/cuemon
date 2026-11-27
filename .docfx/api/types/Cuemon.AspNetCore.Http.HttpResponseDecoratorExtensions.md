---
uid: Cuemon.AspNetCore.Http.HttpResponseDecoratorExtensions
example:
- *content
---

The following example demonstrates how to add ETag and Last-Modified HTTP response headers using the <xref:Cuemon.AspNetCore.Http.HttpResponseDecoratorExtensions> class accessed through the <xref:Cuemon.Decorator> class.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;

var context = new DefaultHttpContext();
var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
builder.CombineWith(Encoding.UTF8.GetBytes("content-data"));

Decorator.Enclose(context.Response).AddOrUpdateEntityTagHeader(context.Request, builder);
var lastModified = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
Decorator.Enclose(context.Response).AddOrUpdateLastModifiedHeader(context.Request, lastModified);

Console.WriteLine(context.Response.Headers.ETag.ToString());
Console.WriteLine(context.Response.Headers.LastModified.ToString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d995af415206ec341978fd65"><a href="~/run-actions/d995af415206ec341978fd65/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d995af415206ec341978fd65/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
