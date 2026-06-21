---
uid: Cuemon.Extensions.Text.Json.Formatters.JsonFormatterOptions
example:
- *content
---

The following example demonstrates how to configure <xref:Cuemon.Extensions.Text.Json.Formatters.JsonFormatterOptions> and use it with the JSON formatter to serialize objects.

```csharp
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;

namespace MyApp.Examples;

public class JsonFormatterOptionsExample
{
    public void SerializeWithCustomOptions()
    {
        // Create options with custom JSON serializer settings
        var options = new JsonFormatterOptions
        {
            Settings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            },
            SensitivityDetails = FaultSensitivityDetails.None
        };

        // Add custom converters
        options.Settings.Converters.Add(new JsonStringEnumConverter());

        var formatter = new JsonFormatter(options);
        var payload = new { UserName = "johndoe", Email = "john@example.com", Age = 30 };

        using (var stream = formatter.Serialize(payload))
        using (var reader = new StreamReader(stream))
        {
            string json = reader.ReadToEnd();
            Console.WriteLine(json);
            // Output:
            // {
            //   "userName": "johndoe",
            //   "email": "john@example.com",
            //   "age": 30
            // }
        }
    }

    public void ConfigureDefaultMediaType()
    {
        // Access the default media type for JSON
        Console.WriteLine(JsonFormatterOptions.DefaultMediaType); // "application/json"

        // DefaultConverters are applied automatically at static initialization
        Console.WriteLine($"Default converters configured: {JsonFormatterOptions.DefaultConverters != null}"); // true
    }

    public void ValidateOptions()
    {
        var options = new JsonFormatterOptions();
        // ValidateOptions throws InvalidOperationException if Settings is null
        options.ValidateOptions();
        Console.WriteLine("Options are valid.");
    }
}
```
