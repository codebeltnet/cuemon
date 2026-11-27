---
uid: Cuemon.Extensions.Text.Json.Converters.DateTimeConverter
example:
- *content
---

`DateTimeConverter` enables custom format and culture-aware serialization of `DateTime` values in `System.Text.Json`. This example registers the converter in `JsonSerializerOptions` with the French date format `"dd/MM/yyyy"` and `fr-FR` culture, then serializes a UTC `DateTime` (`2026-06-16`). The JSON output contains the date as `"16/06/2026"`. Deserializing the same JSON back to a `DateTime` and formatting it again with `"dd/MM/yyyy"` produces `"16/06/2026"`, confirming round-trip fidelity with culture-aware formatting.

```csharp
// Program.cs
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json.Converters;

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
};

// Register a converter that writes dates in the French "dd/MM/yyyy" format
options.Converters.Add(new DateTimeConverter("dd/MM/yyyy", new CultureInfo("fr-FR")));

var original = new DateTime(2026, 6, 16, 14, 30, 0, DateTimeKind.Utc);

string json = JsonSerializer.Serialize(original, options);
Console.WriteLine(json); // "16/06/2026"

var restored = JsonSerializer.Deserialize<DateTime>(json, options);
Console.WriteLine(restored.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)); // 16/06/2026
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="23f5f166aca856278ab12087"><a href="~/run-actions/23f5f166aca856278ab12087/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/23f5f166aca856278ab12087/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
