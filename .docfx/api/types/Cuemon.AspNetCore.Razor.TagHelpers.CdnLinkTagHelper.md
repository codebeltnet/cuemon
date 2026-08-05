---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnLinkTagHelper
example:
- *content
---

The following example demonstrates how to create `CdnLinkTagHelper` with CDN-specific options.

```csharp
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

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

```
