---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppImageTagHelper
example:
- *content
---

The following example demonstrates how to create `AppImageTagHelper` with application-scoped options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using MyApp.Examples;

AppImageTagHelperExample.Demonstrate();

namespace MyApp.Examples
{
    public static class AppImageTagHelperExample
    {
        public static void Demonstrate()
        {
            var options = Options.Create(CreateOptions());

            var tagHelper = new AppImageTagHelper(options)
            {
                Src = "images/logo.svg",
                Alt = "Codebelt logo"
            };

            Console.WriteLine(FormatAssetUrl(tagHelper.Options, tagHelper.Src));
        }

        private static AppTagHelperOptions CreateOptions() => new()
        {
            Scheme = ProtocolUriScheme.Relative,
            BaseUrl = "static.cuemon.net"
        };

        private static string FormatAssetUrl(AppTagHelperOptions options, string asset)
        {
            return options.GetFormattedBaseUrl() + asset;
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6a2510c253e8a562ea66e53c"><a href="~/run-actions/6a2510c253e8a562ea66e53c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6a2510c253e8a562ea66e53c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
