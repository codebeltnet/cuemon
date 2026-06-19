---
uid: Cuemon.Text.UriStringOptions
example:
- *content
---

The following example demonstrates how to configure UriStringOptions to validate and inspect URI properties including kind and allowed schemes.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Text;

namespace Contoso.Webhooks;

public sealed class UriStringOptionsExample
{
    public static void Run()
    {
        var options = new UriStringOptions
        {
            Kind = UriKind.Absolute,
            Schemes = new List<UriScheme> { UriScheme.Https, UriScheme.Http }
        };

        options.ValidateOptions();

        bool allowsHttps = options.Schemes.Contains(UriScheme.Https);
        int knownSchemes = UriStringOptions.AllUriSchemes.Count();

        Console.WriteLine($"Kind: {options.Kind}");
        Console.WriteLine($"Allows HTTPS: {allowsHttps}");
        Console.WriteLine($"Known schemes: {knownSchemes}");
    }
}
```
