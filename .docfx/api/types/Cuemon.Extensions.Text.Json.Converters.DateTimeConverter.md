---
uid: Cuemon.Extensions.Text.Json.Converters.DateTimeConverter
example:
- *content
---

`DateTimeConverter` enables custom format and culture-aware serialization of `DateTime` values in `System.Text.Json`. This example registers the converter in `JsonSerializerOptions` with the French date format `"dd/MM/yyyy"` and `fr-FR` culture, then serializes a UTC `DateTime` (`2026-06-16`). The JSON output contains the date as `"16/06/2026"`. Deserializing the same JSON back to a `DateTime` and formatting it again with `"dd/MM/yyyy"` produces `"16/06/2026"`, confirming round-trip fidelity with culture-aware formatting.

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
