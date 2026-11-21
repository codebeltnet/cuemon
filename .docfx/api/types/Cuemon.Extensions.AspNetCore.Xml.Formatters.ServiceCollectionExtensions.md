---
uid: Cuemon.Extensions.AspNetCore.Xml.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register XML formatter options and an XML-based exception response formatter in the ASP.NET Core service collection.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddXmlFormatterOptions(options =>
{
    options.SynchronizeWithXmlConvert = true;
});
services.AddXmlExceptionResponseFormatter();

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
var formatters = provider.GetServices<IHttpExceptionDescriptorResponseFormatter>().ToList();

Console.WriteLine(options.SynchronizeWithXmlConvert);
Console.WriteLine(formatters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="455fdc5793763e974d21a5e3"><a href="~/run-actions/455fdc5793763e974d21a5e3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/455fdc5793763e974d21a5e3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
