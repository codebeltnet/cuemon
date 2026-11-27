---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppLinkTagHelper
example:
- *content
---

The following example demonstrates how to create <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.AppLinkTagHelper"/> with application-scoped options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using MyApp.Examples;

AppLinkTagHelperExample.Demonstrate();

namespace MyApp.Examples
{
    public static class AppLinkTagHelperExample
    {
        private sealed class StylesheetVersion : ICacheBusting
        {
            public string Version => "1.0.0";
        }

        public static void Demonstrate()
        {
            var options = new AppTagHelperOptions
            {
                Scheme = ProtocolUriScheme.Relative,
                BaseUrl = "static.cuemon.net"
            };

            var version = new StylesheetVersion();
            var tagHelper = new AppLinkTagHelper(Options.Create(options), version)
            {
                Href = "css/site.css"
            };
            var stylesheetHref = $"{tagHelper.Options.GetFormattedBaseUrl()}{tagHelper.Href}?v={version.Version}";

            Console.WriteLine(stylesheetHref);
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8fc8d5f63811d11d5219c0e5"><a href="~/run-actions/8fc8d5f63811d11d5219c0e5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8fc8d5f63811d11d5219c0e5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
