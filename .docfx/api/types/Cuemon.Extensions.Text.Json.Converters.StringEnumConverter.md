---
uid: Cuemon.Extensions.Text.Json.Converters.StringEnumConverter
example:
- *content
---

The following example demonstrates how to use the <see cref="Cuemon.Extensions.Text.Json.Converters.StringEnumConverter"/> to serialize and deserialize non-flags enum values as their string representation rather than their underlying integer value.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;

namespace MyApp.Examples;

public class StringEnumConverterExample
{
    public void Demonstrate()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new StringEnumConverter());

        var payload = new { Day = DayOfWeek.Friday, Status = UriKind.Relative };

        string json = JsonSerializer.Serialize(payload, options);
        Console.WriteLine(json);
        // { "day": "Friday", "status": "Relative" }

        var restored = JsonSerializer.Deserialize<Payload>(json, options);
        Console.WriteLine(restored.Day); // Friday
    }

    public class Payload
    {
        public DayOfWeek Day { get; set; }
        public UriKind Status { get; set; }
    }
}
```
