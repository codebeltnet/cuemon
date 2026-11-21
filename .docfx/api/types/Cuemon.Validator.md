---
uid: Cuemon.Validator
example:
- *content
---

The following example demonstrates how to use the `Validator` class to guard against null, empty, and invalid arguments using precondition checks.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Contoso.Routing;

ValidatorExample.Run();

namespace Contoso.Routing
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="20370480b604ca996bdc483d"><a href="~/run-actions/20370480b604ca996bdc483d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/20370480b604ca996bdc483d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
