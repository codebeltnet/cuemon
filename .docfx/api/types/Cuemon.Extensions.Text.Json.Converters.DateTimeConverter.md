---
uid: Cuemon.Extensions.Text.Json.Converters.DateTimeConverter
example:
- *content
---

The following example demonstrates how to register and use the <see cref="Cuemon.Extensions.Text.Json.Converters.DateTimeConverter"/> to serialize and deserialize <see cref="DateTime"/> values with a custom format and culture-specific formatting.

```csharp
using System;
using System.Globalization;
using System.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;

namespace MyApp.Examples;

public class DateTimeConverterExample
{
    public void Demonstrate()
    {
        var options = new JsonSerializerOptions();

        // Register a converter that writes dates in the French "dd/MM/yyyy" format
        options.Converters.Add(new DateTimeConverter("dd/MM/yyyy", new CultureInfo("fr-FR")));

        var original = new DateTime(2026, 6, 16, 14, 30, 0, DateTimeKind.Utc);

        string json = JsonSerializer.Serialize(original, options);
        Console.WriteLine(json); // "16/06/2026"

        var restored = JsonSerializer.Deserialize<DateTime>(json, options);
        Console.WriteLine(restored.ToString("dd/MM/yyyy")); // 16/06/2026

}
}

```
