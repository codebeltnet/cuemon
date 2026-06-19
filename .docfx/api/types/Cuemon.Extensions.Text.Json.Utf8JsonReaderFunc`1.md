---
uid: Cuemon.Extensions.Text.Json.Utf8JsonReaderFunc`1
example:
- *content
---

The following example demonstrates how a <see cref="Utf8JsonReaderFunc{T}" /> can deserialize a value inside a dynamic JSON converter.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace MyApp.Examples;

public static class Utf8JsonReaderFuncExample
{
    public static void Demonstrate()
    {
        Utf8JsonReaderFunc<Guid> reader = (ref Utf8JsonReader jsonReader, Type _, JsonSerializerOptions __) =>
            Guid.Parse(jsonReader.GetString());

        var converter = DynamicJsonConverter.Create<Guid>(reader: reader);
        var result = JsonSerializer.Deserialize<Guid>("\"11111111-2222-3333-4444-555555555555\"", new JsonSerializerOptions
        {
            Converters = { converter }
        });

        Console.WriteLine(result);
    }
}
```
