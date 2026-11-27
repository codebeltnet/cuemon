---
uid: Cuemon.Extensions.AspNetCore.Data.Integrity.ChecksumBuilderExtensions
example:
- *content
---

The following example demonstrates how to create an `EntityTagHeaderValue` from a `ChecksumBuilder` instance.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Data.Integrity;
using Cuemon.Security;

var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
builder.CombineWith(Encoding.UTF8.GetBytes("content-data"));

var strongTag = builder.ToEntityTagHeaderValue();
var weakTag = builder.ToEntityTagHeaderValue(isWeak: true);

Console.WriteLine(strongTag.Tag.ToString());
Console.WriteLine(weakTag.IsWeak);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="47c3dab1c5f689db98d9745e"><a href="~/run-actions/47c3dab1c5f689db98d9745e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/47c3dab1c5f689db98d9745e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
