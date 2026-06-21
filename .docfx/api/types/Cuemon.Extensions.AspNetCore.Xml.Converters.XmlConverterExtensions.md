---
uid: Cuemon.Extensions.AspNetCore.Xml.Converters.XmlConverterExtensions
example:
- *content
---

The following example demonstrates how to register ASP.NET Core-friendly XML converters on an <see cref="XmlSerializerOptions" /> instance.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples
{
public static class XmlConverterExtensionsExample
{
    public static void Demonstrate()
    {
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
    }
}
}
```
