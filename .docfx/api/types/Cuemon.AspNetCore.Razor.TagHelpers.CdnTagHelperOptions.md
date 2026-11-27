---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnTagHelperOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.CdnTagHelperOptions"/> for CDN-scoped tag helpers.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;

var options = new CdnTagHelperOptions
{
    Scheme = ProtocolUriScheme.Https,
    BaseUrl = "nblcdn.net"
};

Console.WriteLine(options.GetFormattedBaseUrl());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="da7bcefb630db38e68fe334c"><a href="~/run-actions/da7bcefb630db38e68fe334c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/da7bcefb630db38e68fe334c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
