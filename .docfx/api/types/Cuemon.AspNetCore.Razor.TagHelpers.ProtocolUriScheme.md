---
uid: Cuemon.AspNetCore.Razor.TagHelpers.ProtocolUriScheme
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.ProtocolUriScheme"/> enum when configuring tag helper options.

```csharp
using Cuemon.AspNetCore.Razor.TagHelpers;
using System;

namespace DocfxExamples;

public class ProtocolUriSchemeExample
{
    public void Demonstrate()
    {
        var options = new CdnTagHelperOptions
        {
            Scheme = ProtocolUriScheme.Https,
            BaseUrl = "cdn.example.com"
        };

        var formatted = options.GetFormattedBaseUrl();
        Console.WriteLine(formatted); // Output: https://cdn.example.com/

}
}

```
