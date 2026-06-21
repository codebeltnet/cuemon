---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnImageTagHelper
example:
- *content
---

The following example demonstrates how to create `CdnImageTagHelper` with CDN-specific options.

```csharp
using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

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
        var tagHelper = new CdnImageTagHelper(Options.Create(settings), version);
        var imageUrl = settings.GetFormattedBaseUrl() + "images/logo.svg?v=" + version.Version;

        Console.WriteLine(imageUrl);
        Console.WriteLine(tagHelper.GetType().Name);
    }
}

```
