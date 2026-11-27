---
uid: Cuemon.Text.EnumStringOptions
example:
- *content
---

The following example demonstrates how to configure `EnumStringOptions` to control case sensitivity when parsing an enum from a string using `ParserFactory.FromEnum`.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Text;

var parser = ParserFactory.FromEnum();

var options = new EnumStringOptions
{
    IgnoreCase = true
};

var result = (UriKind)parser.Parse("relative", typeof(UriKind), o =>
{
    o.IgnoreCase = options.IgnoreCase;
});

Console.WriteLine(result); // outputs: Relative
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="58cdffbec5c2fa4e5b59be8b"><a href="~/run-actions/58cdffbec5c2fa4e5b59be8b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/58cdffbec5c2fa4e5b59be8b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
