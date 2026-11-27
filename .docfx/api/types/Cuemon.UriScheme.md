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

Console.WriteLine(httpsScheme); // outputs: https
Console.WriteLine(ftpScheme);   // outputs: ftp
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="71452aa72ff34c80c40284ba"><a href="~/run-actions/71452aa72ff34c80c40284ba/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/71452aa72ff34c80c40284ba/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
