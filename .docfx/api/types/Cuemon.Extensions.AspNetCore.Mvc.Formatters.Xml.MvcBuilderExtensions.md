---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to add XML serialization formatters to an MVC builder. It calls `AddXmlFormatters` and `AddXmlFormattersOptions` on the builder returned by `AddControllers`, then resolves `IOptions<MvcOptions>`, `IOptions<XmlFormatterOptions>`, and the XML exception response formatter to verify the configuration.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddLogging();
services.AddFaultDescriptorOptions();
var builder = services.AddControllers();

builder.AddXmlFormatters();

builder.AddXmlFormattersOptions(options =>
{
    options.Settings.Writer.Indent = true;
});

using var provider = services.BuildServiceProvider();
var mvcOptions = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
var xmlOptions = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
var exceptionFormatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>>();

Console.WriteLine($"Has XML input formatter: {mvcOptions.InputFormatters.OfType<XmlSerializationInputFormatter>().Any()}");
Console.WriteLine($"Has XML output formatter: {mvcOptions.OutputFormatters.OfType<XmlSerializationOutputFormatter>().Any()}");
Console.WriteLine($"Indent: {xmlOptions.Settings.Writer.Indent}");
Console.WriteLine($"Exception formatter resolved: {exceptionFormatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7b7747323e0c742e4335f010"><a href="~/run-actions/7b7747323e0c742e4335f010/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7b7747323e0c742e4335f010/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
