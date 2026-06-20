---
uid: Cuemon.Extensions.Text.Json.Converters.ExceptionConverter
example:
- *content
---

`ExceptionConverter` serializes `Exception` instances to JSON, including optional stack trace and `Data` dictionary content. This example configures `JsonSerializerOptions` with `WriteIndented = true` and adds the converter with `includeStackTrace: true` and `includeData: true`, then creates a nested `InvalidOperationException("Outer operation failed.")` with an inner `InvalidOperationException("Inner operation failed.")` and a `CorrelationId` data entry. The resulting JSON includes top-level fields (`Type`, `Source`, `Message`, `Stack`, `Data`) and a nested `Inner` section for the inner exception with its own `Type` and `Message`. Console output displays the complete JSON structure.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;

namespace MyApp.Examples;

public class ExceptionConverterExample
{
    public void Demonstrate()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Include both stack trace and exception data
        options.Converters.Add(new ExceptionConverter(includeStackTrace: true, includeData: true));

        var inner = new InvalidOperationException("Inner operation failed.");
        var ex = new InvalidOperationException("Outer operation failed.", inner);
        ex.Data["CorrelationId"] = "abc-123";

        string json = JsonSerializer.Serialize(ex, options);
        Console.WriteLine(json);
        // {
        //   "Type": "System.InvalidOperationException",
        //   "Source": "...",
        //   "Message": "Outer operation failed.",
        //   "Stack": [ "   at ..." ],
        //   "Data": { "CorrelationId": "abc-123" },
        //   "Inner": {
        //     "Type": "System.InvalidOperationException",
        //     "Message": "Inner operation failed."
        //   }
        // }

}
}

```
