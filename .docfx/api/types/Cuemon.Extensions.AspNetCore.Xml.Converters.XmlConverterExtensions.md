---
uid: Cuemon.Extensions.AspNetCore.Xml.Converters.XmlConverterExtensions
example:
- *content
---

The following example demonstrates how to register ASP.NET Core-friendly XML converters on an <see cref="XmlSerializerOptions" /> instance.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization;

var converters = new List<XmlConverter>();
converters.AddProblemDetailsConverter();
converters.AddHttpExceptionDescriptorConverter(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
converters.AddStringValuesConverter();
converters.AddHeaderDictionaryConverter();
converters.AddQueryCollectionConverter();
converters.AddFormCollectionConverter();
converters.AddCookieCollectionConverter();

var serializerOptions = new XmlSerializerOptions();
foreach (var converter in converters)
{
    serializerOptions.Converters.Add(converter);
}

Console.WriteLine(serializerOptions.Converters.Count);
Console.WriteLine(serializerOptions.Converters[0].CanConvert(typeof(object)));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="697c8a8eded4b7a351a6d40c"><a href="~/run-actions/697c8a8eded4b7a351a6d40c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/697c8a8eded4b7a351a6d40c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
