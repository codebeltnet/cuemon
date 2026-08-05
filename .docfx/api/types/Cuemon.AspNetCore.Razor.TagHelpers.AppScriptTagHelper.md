---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppScriptTagHelper
example:
- *content
---

The following example demonstrates how to create <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.AppScriptTagHelper"/> with application-scoped options.

```csharp
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class AppScriptTagHelperExample
{
    public static void Demonstrate()
    {
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
    }
}

```
