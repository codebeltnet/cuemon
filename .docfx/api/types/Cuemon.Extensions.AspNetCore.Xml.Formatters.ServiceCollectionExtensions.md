---
uid: Cuemon.Extensions.AspNetCore.Xml.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register XML formatter options and an XML-based exception response formatter in the ASP.NET Core service collection, then resolves both from the service provider.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddFaultDescriptorOptions();
services.AddXmlFormatterOptions(options =>
{
    options.SynchronizeWithXmlConvert = true;
});
services.AddXmlExceptionResponseFormatter();

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
var formatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>>();

Console.WriteLine(options.SynchronizeWithXmlConvert);
Console.WriteLine($"Formatter resolved: {formatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ddd9758bdf88975ad4f53550"><a href="~/run-actions/ddd9758bdf88975ad4f53550/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ddd9758bdf88975ad4f53550/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
