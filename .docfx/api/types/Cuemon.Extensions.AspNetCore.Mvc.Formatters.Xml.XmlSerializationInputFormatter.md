---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationInputFormatter
example:
- *content
---

The following example demonstrates how to construct an <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationInputFormatter"/> and inspect its supported media types and encodings.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;

var options = new XmlFormatterOptions();
var formatter = new XmlSerializationInputFormatter(options);

Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="63bfda4be52013e253f299c7"><a href="~/run-actions/63bfda4be52013e253f299c7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/63bfda4be52013e253f299c7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
