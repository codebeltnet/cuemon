---
uid: Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensions> to configure a custom <see cref="T:System.Text.Json.JsonSerializerOptions"/> with specialized converters.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {
        var converters = new List<JsonConverter>();
        converters.AddDateTimeConverter("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        converters.AddStringEnumConverter();
        converters.AddStringFlagsEnumConverter();
        converters.AddExceptionConverter(true, false);
        converters.AddFailureConverter();
        converters.AddTransientFaultExceptionConverter();
        converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(
            o => o.SensitivityDetails = FaultSensitivityDetails.All);
        converters.AddDataPairConverter();
        converters.RemoveAllOf<string>();
        converters.RemoveAllOf(typeof(TimeSpan));

        var formatter = new JsonFormatter(o =>
        {
            o.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            foreach (var converter in converters)
            {
                o.Settings.Converters.Add(converter);
            }
        });

        var person = new { FullName = "Alice Johnson", BirthDate = new DateTime(1990, 6, 15) };
        using var jsonStream = JsonFormatter.SerializeObject(person, options =>
        {
            options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            foreach (var converter in converters)
            {
                options.Settings.Converters.Add(converter);
            }
        });
        jsonStream.Position = 0;
        using var reader = new System.IO.StreamReader(jsonStream);
        string json = reader.ReadToEnd();
        Console.WriteLine(json);
    }
}

```
