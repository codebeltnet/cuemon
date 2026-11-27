---
uid: Cuemon.Extensions.Text.Json.DynamicJsonConverter
example:
- *content
---

The following example shows how to register a custom JSON converter for the `Version` type using `DynamicJsonConverter`. It serializes a version to JSON and deserializes it back, printing each result.

```csharp
// Program.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json;

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
};
options.Converters.Add(DynamicJsonConverter.Create<Version>(
    writer: (utf8Writer, value, _) => utf8Writer.WriteStringValue(value.ToString()),
    reader: (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions jsonOptions) => Version.Parse(reader.GetString())
));

var version = new Version("5.0.0");
string json = JsonSerializer.Serialize(version, options);
Console.WriteLine(json);

var deserialized = JsonSerializer.Deserialize<Version>("\"6.0.0\"", options);
Console.WriteLine(deserialized);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="21ef239bffe3560880cf78ba"><a href="~/run-actions/21ef239bffe3560880cf78ba/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/21ef239bffe3560880cf78ba/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
