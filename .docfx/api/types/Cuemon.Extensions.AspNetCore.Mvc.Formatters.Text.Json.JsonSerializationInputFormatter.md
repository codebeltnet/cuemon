---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationInputFormatter
example:
- *content
---

The following example demonstrates how to construct a <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationInputFormatter"/> and inspect its supported media types and encodings.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;

var options = new JsonFormatterOptions();
var formatter = new JsonSerializationInputFormatter(options);

Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cb044ae61458e41ca68933d5"><a href="~/run-actions/cb044ae61458e41ca68933d5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cb044ae61458e41ca68933d5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
