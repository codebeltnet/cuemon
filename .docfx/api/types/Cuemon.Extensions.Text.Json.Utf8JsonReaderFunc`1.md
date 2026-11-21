---
uid: Cuemon.Extensions.Text.Json.Utf8JsonReaderFunc`1
example:
- *content
---

The following example demonstrates how a <see cref="Utf8JsonReaderFunc{T}" /> can deserialize a value inside a dynamic JSON converter.

```csharp
// Program.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json;

Utf8JsonReaderFunc<Guid> reader = (ref Utf8JsonReader jsonReader, Type _, JsonSerializerOptions __) =>
    Guid.Parse(jsonReader.GetString());

var converter = DynamicJsonConverter.Create<Guid>(reader: reader);
var result = JsonSerializer.Deserialize<Guid>("\"11111111-2222-3333-4444-555555555555\"", new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    Converters = { converter }
});

Console.WriteLine(result);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="85f77b7511583d36305c3374"><a href="~/run-actions/85f77b7511583d36305c3374/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/85f77b7511583d36305c3374/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
