---
uid: Cuemon.Extensions.Text.Json.Converters.StringEnumConverter
example:
- *content
---

`StringEnumConverter` serializes and deserializes non-flags enum values as their string names rather than underlying integer values in `System.Text.Json`. This example creates `JsonSerializerOptions` with `CamelCase` naming policy and adds the converter, then serializes an anonymous object with `DayOfWeek.Friday` and `UriKind.Relative`. The JSON output shows `"friday"` and `"relative"` instead of numeric values like `5` or `2`. Deserializing the JSON back into a typed `Payload` object confirms that the string values round-trip correctly to the original enum members, with `restored.Day` output as `Friday`.

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
