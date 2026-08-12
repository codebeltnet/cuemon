---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnScriptTagHelper
example:
- *content
---

The following example demonstrates how to create `CdnScriptTagHelper` with CDN-specific options.

```csharp
using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class CdnScriptTagHelperExample
{
    private sealed class ScriptVersion : ICacheBusting
    {
        public string Version => "1.0.0";
    }

    public static void Demonstrate()
    {
        var options = Options.Create(new CdnTagHelperOptions
        {
            Scheme = ProtocolUriScheme.Https,
            BaseUrl = "nblcdn.net"
        });

        var cacheBusting = new ScriptVersion();
        var tagHelper = new CdnScriptTagHelper(options, cacheBusting)
        {
            Src = "packages/fontawesome/5.15.3/js/all.js",
            Defer = true
        };
        var scriptPath = string.Concat(tagHelper.Options.GetFormattedBaseUrl(), tagHelper.Src, "?v=", cacheBusting.Version);

        Console.WriteLine(scriptPath);
    }
}

```
