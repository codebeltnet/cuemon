---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnImageTagHelper
example:
- *content
---

The following example demonstrates how to create `CdnImageTagHelper` with CDN-specific options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using MyApp.Examples;

CdnImageTagHelperExample.Demonstrate();

namespace MyApp.Examples
{
    public static class CdnImageTagHelperExample
    {
        private sealed class AssetVersion : ICacheBusting
        {
            public string Version => "2.1.0";
        }

        public static void Demonstrate()
        {
            var settings = new CdnTagHelperOptions
            {
                Scheme = ProtocolUriScheme.Https,
                BaseUrl = "nblcdn.net"
            };

            var version = new AssetVersion();
            var tagHelper = new CdnImageTagHelper(Options.Create(settings), version)
            {
                Src = "images/logo.svg",
                Alt = "Codebelt logo"
            };
            var imageUrl = tagHelper.Options.GetFormattedBaseUrl() + tagHelper.Src + "?v=" + version.Version;

            Console.WriteLine(imageUrl);
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c3eaf28dee6b2538f764dcf9"><a href="~/run-actions/c3eaf28dee6b2538f764dcf9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c3eaf28dee6b2538f764dcf9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
