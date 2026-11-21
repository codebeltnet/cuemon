---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationMvcOptionsSetup
example:
- *content
---

The following example demonstrates how <see cref="XmlSerializationMvcOptionsSetup" /> adds XML serialization formatters to <see cref="Microsoft.AspNetCore.Mvc.MvcOptions" />.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var formatterOptions = Options.Create(new XmlFormatterOptions());
var setup = new XmlSerializationMvcOptionsSetup(formatterOptions);
var mvcOptions = new MvcOptions();

setup.Configure(mvcOptions);

Console.WriteLine(mvcOptions.InputFormatters.Count);
Console.WriteLine(mvcOptions.OutputFormatters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a2d06bba70c21f21095e720a"><a href="~/run-actions/a2d06bba70c21f21095e720a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a2d06bba70c21f21095e720a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
