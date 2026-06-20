---
uid: Cuemon.Extensions.Text.Json.Converters.StringFlagsEnumConverter
example:
- *content
---

`StringFlagsEnumConverter` serializes and deserializes `[Flags]` enum values as an array of active flag string names instead of a single integer. This example registers the converter in `JsonSerializerOptions`, then serializes `FileShare.Read | FileShare.Write` which produces the JSON array `["Read", "Write"]`. Deserializing the array back to a `FileShare` value restores the combined flags, and the output displays `Read, Write`, confirming round-trip correctness for flags enum values.

```csharp
using System;
using System.IO;
using System.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;

namespace MyApp.Examples;

public class StringFlagsEnumConverterExample
{
    public void Demonstrate()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new StringFlagsEnumConverter());

        var value = FileShare.Read | FileShare.Write;

        string json = JsonSerializer.Serialize(value, options);
        Console.WriteLine(json);
        // ["Read", "Write"]

        var restored = JsonSerializer.Deserialize<FileShare>(json, options);
        Console.WriteLine(restored); // Read, Write

}
}

```
