---
uid: Cuemon.Extensions.Text.Json.Utf8JsonWriterAction`1
example:
- *content
---

The following example demonstrates how a <see cref="Utf8JsonWriterAction{T}" /> can serialize a value inside a dynamic JSON converter.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace MyApp.Examples;

public static class Utf8JsonWriterActionExample
{
    public static void Demonstrate()
    {
        Utf8JsonWriterAction<Guid> writer = (jsonWriter, value, _) =>
            jsonWriter.WriteStringValue(value.ToString("D"));

        var converter = DynamicJsonConverter.Create<Guid>(writer: writer);
        var json = JsonSerializer.Serialize(Guid.Parse("11111111-2222-3333-4444-555555555555"), new JsonSerializerOptions
        {
            Converters = { converter }
        });

        Console.WriteLine(json);
    }
}
```
