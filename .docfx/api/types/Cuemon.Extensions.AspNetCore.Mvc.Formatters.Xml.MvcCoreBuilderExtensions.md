---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions
example:
- *content
---

The following example requires `Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml` and sets up a `ServiceCollection` with logging and `AddFaultDescriptorOptions()`, then calls `AddXmlFormatters` and `AddXmlFormattersOptions` from <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions"/> on `AddMvcCore()`. It builds the `ServiceProvider` and resolves `IOptions<MvcOptions>`, `IOptions<XmlFormatterOptions>`, and `HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>`. As the outcome, the console output confirms that XML input/output formatters, `SynchronizeWithXmlConvert = true`, and the XML exception formatter are registered.

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
var builder = services.AddMvcCore();

builder.AddXmlFormatters(options =>
{
    options.SynchronizeWithXmlConvert = true;
});

builder.AddXmlFormattersOptions(options =>
{
    options.SynchronizeWithXmlConvert = true;
});

using var provider = services.BuildServiceProvider();
var mvcOptions = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
var xmlOptions = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
var exceptionFormatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>>();

Console.WriteLine($"Has XML input formatter: {mvcOptions.InputFormatters.OfType<XmlSerializationInputFormatter>().Any()}");
Console.WriteLine($"Has XML output formatter: {mvcOptions.OutputFormatters.OfType<XmlSerializationOutputFormatter>().Any()}");
Console.WriteLine($"SynchronizeWithXmlConvert: {xmlOptions.SynchronizeWithXmlConvert}");
Console.WriteLine($"Exception formatter resolved: {exceptionFormatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="238fee7ee663007eac1449cf"><a href="~/run-actions/238fee7ee663007eac1449cf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/238fee7ee663007eac1449cf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
