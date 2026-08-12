---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppImageTagHelper
example:
- *content
---

The following example demonstrates how to create `AppImageTagHelper` with application-scoped options.

```csharp
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

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
```
