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

        var tagHelper = new CdnLinkTagHelper(options);
        var stylesheetHref = options.Value.GetFormattedBaseUrl() + "packages/fontawesome/5.15.3/css/all.css";

        Console.WriteLine(stylesheetHref);
        Console.WriteLine(tagHelper.GetType().Name);
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
