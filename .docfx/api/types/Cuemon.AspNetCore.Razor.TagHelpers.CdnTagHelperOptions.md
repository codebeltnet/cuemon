---
uid: Cuemon.AspNetCore.Razor.TagHelpers.CdnTagHelperOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.CdnTagHelperOptions"/> for CDN-scoped tag helpers.

```csharp
using System;
using Cuemon.AspNetCore.Razor.TagHelpers;

namespace MyApp.Examples;

public static class CdnTagHelperOptionsExample
{
    public static void Demonstrate()
    {
        var options = new CdnTagHelperOptions
        {
            Scheme = ProtocolUriScheme.Https,
            BaseUrl = "nblcdn.net"
        };

        Console.WriteLine(options.GetFormattedBaseUrl());
    }
}

```
