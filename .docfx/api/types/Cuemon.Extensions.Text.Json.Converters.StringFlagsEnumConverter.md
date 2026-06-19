---
uid: Cuemon.Extensions.Text.Json.Converters.StringFlagsEnumConverter
example:
- *content
---

The following example demonstrates how to use the <see cref="Cuemon.Extensions.Text.Json.Converters.StringFlagsEnumConverter"/> to serialize and deserialize enum values decorated with <see cref="FlagsAttribute"/> as an array of strings.

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
