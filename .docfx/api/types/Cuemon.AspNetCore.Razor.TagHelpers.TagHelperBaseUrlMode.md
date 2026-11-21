---
uid: Cuemon.AspNetCore.Razor.TagHelpers.TagHelperBaseUrlMode
example:
- *content
---

The `TagHelperBaseUrlMode` enumeration controls how the base URL of a static resource is resolved by the app- and CDN-scoped tag helpers. `Configured` (the default) resolves the base URL exclusively from the configured `BaseUrl` and `Scheme`, whereas `Automatic` uses the configured `BaseUrl` when present and otherwise derives the base URL from the current HTTP request — useful when the same deployment is served from multiple origins. The following example demonstrates both modes through `CdnTagHelperOptions` and `GetFormattedBaseUrl`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;

var configured = new CdnTagHelperOptions
{
    BaseUrlMode = TagHelperBaseUrlMode.Configured,
    Scheme = ProtocolUriScheme.Https,
    BaseUrl = "static.example.com"
};
Console.WriteLine(configured.GetFormattedBaseUrl()); // Output: https://static.example.com/

var automatic = new CdnTagHelperOptions
{
    BaseUrlMode = TagHelperBaseUrlMode.Automatic,
    BaseUrl = null // No fixed origin, so fall back to the current HTTP request.
};
var request = new DefaultHttpContext().Request;
request.Scheme = "https";
request.Host = new HostString("app.example.com");
request.PathBase = "/myapp";
Console.WriteLine(automatic.GetFormattedBaseUrl(request)); // Output: https://app.example.com/myapp/
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fa98cc084b8c266f00294224"><a href="~/run-actions/fa98cc084b8c266f00294224/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fa98cc084b8c266f00294224/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
