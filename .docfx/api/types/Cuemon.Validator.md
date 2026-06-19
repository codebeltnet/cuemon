---
uid: Cuemon.Validator
example:
- *content
---

The following example demonstrates how to use the `Validator` class to guard against null, empty, and invalid arguments using precondition checks.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;

namespace Contoso.Routing;

public sealed class ValidatorExample
{
    public static void Run()
    {
        string endpoint = NormalizeEndpoint("https://api.cuemon.net", new[] { "v1", "health" });
        Console.WriteLine(endpoint);
    }

    private static string NormalizeEndpoint(string endpoint, IEnumerable<string> segments)
    {
        Validator.ThrowIfNullOrWhitespace(endpoint);
        Validator.ThrowIfSequenceNullOrEmpty(segments);
        Validator.ThrowIfNotUri(endpoint, UriKind.Absolute);

        return string.Join("/", new[] { endpoint.TrimEnd('/') }.Concat(segments));
    }
}
```
