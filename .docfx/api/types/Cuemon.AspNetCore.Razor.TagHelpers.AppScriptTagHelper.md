---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppScriptTagHelper
example:
- *content
---

The following example demonstrates how to create <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.AppScriptTagHelper"/> with application-scoped options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

var options = Options.Create(new AppTagHelperOptions
{
    Scheme = ProtocolUriScheme.Relative,
    BaseUrl = "static.cuemon.net"
});

var tagHelper = new AppScriptTagHelper(options)
{
    Src = "js/app.js",
    Defer = true
};

Console.WriteLine(string.Concat(tagHelper.Options.GetFormattedBaseUrl(), tagHelper.Src));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="72a5aef61df037dcf47c696a"><a href="~/run-actions/72a5aef61df037dcf47c696a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/72a5aef61df037dcf47c696a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
