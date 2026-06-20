---
uid: Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensions
example:
- *content
---

`JsonConverterCollectionExtensions` provides fluent extension methods for building a `List<JsonConverter>` with specialized converters and applying them to a `JsonFormatter`. This example calls `AddDateTimeConverter`, `AddStringEnumConverter`, `AddStringFlagsEnumConverter`, `AddExceptionConverter`, `AddFailureConverter`, `AddTransientFaultExceptionConverter`, and `AddDataPairConverter` to register each converter, then demonstrates `RemoveAllOf<string>()` and `RemoveAllOf(typeof(TimeSpan))` to remove converters by type. The converter list is applied to a `JsonFormatter` with `CamelCase` naming policy to serialize an anonymous person object. Console output displays the resulting JSON with the configured formatting.

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
