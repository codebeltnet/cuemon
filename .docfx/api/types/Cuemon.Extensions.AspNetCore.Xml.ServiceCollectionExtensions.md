---
uid: Cuemon.Extensions.AspNetCore.Xml.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal XML formatter options using `AddMinimalXmlOptions` in the ASP.NET Core service collection.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Xml;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.Configure<XmlFormatterOptions>(options =>
{
    options.SynchronizeWithXmlConvert = true;
});
services.AddMinimalXmlOptions(options =>
{
    options.SynchronizeWithXmlConvert = true;
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
Console.WriteLine(options.SynchronizeWithXmlConvert);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1606e801e97aec9f8c33cbf0"><a href="~/run-actions/1606e801e97aec9f8c33cbf0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1606e801e97aec9f8c33cbf0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
