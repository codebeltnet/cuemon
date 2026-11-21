---
uid: Cuemon.Text.UriStringOptions
example:
- *content
---

The following example demonstrates how to configure UriStringOptions to validate and inspect URI properties including kind and allowed schemes.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Text;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2ec062c453287d1f8826ee0f"><a href="~/run-actions/2ec062c453287d1f8826ee0f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2ec062c453287d1f8826ee0f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
