---
uid: Cuemon.AspNetCore.Razor.TagHelpers.ProtocolUriScheme
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.ProtocolUriScheme"/> enum when configuring tag helper options.

```csharp
// Program.cs
using Cuemon.AspNetCore.Razor.TagHelpers;
using System;

var options = new CdnTagHelperOptions
{
    Scheme = ProtocolUriScheme.Https,
    BaseUrl = "cdn.example.com"
};

var formatted = options.GetFormattedBaseUrl();
Console.WriteLine(formatted); // Output: https://cdn.example.com/
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ef6cdd6cb2a12a8ef742b1f8"><a href="~/run-actions/ef6cdd6cb2a12a8ef742b1f8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ef6cdd6cb2a12a8ef742b1f8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
