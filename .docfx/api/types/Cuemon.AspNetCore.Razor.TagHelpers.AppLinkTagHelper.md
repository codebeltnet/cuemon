---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppLinkTagHelper
example:
- *content
---

The following example demonstrates how to create <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.AppLinkTagHelper"/> with application-scoped options.

```csharp
using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

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
        var tagHelper = new AppLinkTagHelper(Options.Create(options), version);
        var stylesheetHref = $"{options.GetFormattedBaseUrl()}css/site.css?v={version.Version}";

        Console.WriteLine(stylesheetHref);
        Console.WriteLine(tagHelper.GetType().Name);
    }
}

```
