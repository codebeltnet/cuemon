---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationOutputFormatter
example:
- *content
---

The following example demonstrates how to construct a <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationOutputFormatter"/> and inspect its supported media types and encodings.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;

var options = new JsonFormatterOptions();
var formatter = new JsonSerializationOutputFormatter(options);

Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c713f7694e44c7009497da2e"><a href="~/run-actions/c713f7694e44c7009497da2e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c713f7694e44c7009497da2e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
