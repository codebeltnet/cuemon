---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnLinkTagHelper
example:
- *content
---

The following example demonstrates how to create `CdnLinkTagHelper` with CDN-specific options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using MyApp.Examples;

CdnLinkTagHelperExample.Demonstrate();

namespace MyApp.Examples
{
    public static class CdnLinkTagHelperExample
    {
        public static void Demonstrate()
        {
            var options = Options.Create(CreateStylesheetOptions("nblcdn.net"));

            var tagHelper = new CdnLinkTagHelper(options)
            {
                Href = "packages/fontawesome/5.15.3/css/all.css"
            };
            var stylesheetHref = tagHelper.Options.GetFormattedBaseUrl() + tagHelper.Href;

            Console.WriteLine(stylesheetHref);
        }

        private static CdnTagHelperOptions CreateStylesheetOptions(string baseUrl)
        {
            return new CdnTagHelperOptions
            {
                Scheme = ProtocolUriScheme.Https,
                BaseUrl = baseUrl
            };
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="70fca55c7c1fa818b79defb9"><a href="~/run-actions/70fca55c7c1fa818b79defb9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/70fca55c7c1fa818b79defb9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
