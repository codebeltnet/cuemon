---
uid: Cuemon.Extensions.AspNetCore.Xml.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal XML formatter options using `AddMinimalXmlOptions` in the ASP.NET Core service collection and resolves the configured `XmlFormatterOptions` and XML exception response formatter.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddFaultDescriptorOptions();
services.AddMinimalXmlOptions(options =>
{
    options.SynchronizeWithXmlConvert = true;
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
var formatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>>();

Console.WriteLine(options.SynchronizeWithXmlConvert);
Console.WriteLine($"Formatter resolved: {formatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e0601b88611e3d9ff3078d95"><a href="~/run-actions/e0601b88611e3d9ff3078d95/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e0601b88611e3d9ff3078d95/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
