---
uid: Cuemon.UriScheme
example:
- *content
---

The following example demonstrates how to use `UriScheme` with `StringFactory.CreateUriScheme` to generate URI scheme strings.

```csharp
// Program.cs
using Cuemon;
using System;

var httpsScheme = StringFactory.CreateUriScheme(UriScheme.Https);
var ftpScheme = StringFactory.CreateUriScheme(UriScheme.Ftp);

Console.WriteLine(httpsScheme); // outputs: https://
Console.WriteLine(ftpScheme);   // outputs: ftp://
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d96fa6934f2cbaf56a5b25ec"><a href="~/run-actions/d96fa6934f2cbaf56a5b25ec/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d96fa6934f2cbaf56a5b25ec/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
