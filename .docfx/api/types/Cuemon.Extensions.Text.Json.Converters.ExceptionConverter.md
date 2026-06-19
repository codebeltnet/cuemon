---
uid: Cuemon.Extensions.Text.Json.Converters.ExceptionConverter
example:
- *content
---

The following example demonstrates how to serialize an <see cref="Exception"/> to JSON including its stack trace and data dictionary.

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
