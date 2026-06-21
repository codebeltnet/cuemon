---
uid: Cuemon.Extensions.Text.Json.DynamicJsonConverter
example:
- *content
---

The following example shows how to register a custom JSON converter for the `Version` type using `DynamicJsonConverter`. It serializes a version to JSON and deserializes it back, printing each result.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace Cuemon.Extensions.Text.Json;

public class DynamicJsonConverterExample
{
    public void Demonstrate()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(DynamicJsonConverter.Create<Version>(
            writer: (utf8Writer, value, _) => utf8Writer.WriteStringValue(value.ToString()),
            reader: (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions jsonOptions) => Version.Parse(reader.GetString())
        ));

        var version = new Version("5.0.0");
        string json = JsonSerializer.Serialize(version, options);
        Console.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Version>("\"6.0.0\"", options);
        Console.WriteLine(deserialized);
    }
}
```
