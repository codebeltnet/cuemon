---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions
example:
- *content
---

The following example demonstrates how to add XML serialization formatters to an MVC core builder using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions"/> extension methods.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
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
Console.WriteLine("MVC Core XML formatters configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5f5f38da9c2c716d9af656df"><a href="~/run-actions/5f5f38da9c2c716d9af656df/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5f5f38da9c2c716d9af656df/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
