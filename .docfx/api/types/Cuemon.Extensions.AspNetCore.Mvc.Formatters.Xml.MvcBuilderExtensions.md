---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to add XML serialization formatters to an MVC builder. It calls `AddXmlFormatters` with indentation enabled, then `AddXmlFormattersOptions` with indentation disabled, on the builder returned by `AddControllers`. Both calls configure the underlying `XmlWriter` settings, showing how to register and customize XML input/output formatters for ASP.NET Core API controllers.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
var builder = services.AddControllers();

builder.AddXmlFormatters(options =>
{
    options.Settings.Writer.Indent = true;
});

builder.AddXmlFormattersOptions(options =>
{
    options.Settings.Writer.Indent = false;
});

using var provider = services.BuildServiceProvider();
Console.WriteLine("MVC XML formatters configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="65c970f33a78e792f4f779c9"><a href="~/run-actions/65c970f33a78e792f4f779c9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/65c970f33a78e792f4f779c9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
