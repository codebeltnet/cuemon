---
uid: Cuemon.Extensions.Text.Json.Utf8JsonWriterAction`1
example:
- *content
---

The following example demonstrates how a <see cref="Utf8JsonWriterAction{T}" /> can serialize a value inside a dynamic JSON converter.

```csharp
// Program.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json;

Utf8JsonWriterAction<Guid> writer = (jsonWriter, value, _) =>
    jsonWriter.WriteStringValue(value.ToString("D"));

var converter = DynamicJsonConverter.Create<Guid>(writer: writer);
var json = JsonSerializer.Serialize(Guid.Parse("11111111-2222-3333-4444-555555555555"), new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    Converters = { converter }
});

Console.WriteLine(json);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6c876f55ceffe837e622bff8"><a href="~/run-actions/6c876f55ceffe837e622bff8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6c876f55ceffe837e622bff8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
