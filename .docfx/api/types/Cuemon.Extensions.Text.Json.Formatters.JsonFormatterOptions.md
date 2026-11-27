---
uid: Cuemon.Extensions.Text.Json.Formatters.JsonFormatterOptions
example:
- *content
---

The following example demonstrates how to configure <xref:Cuemon.Extensions.Text.Json.Formatters.JsonFormatterOptions> and use it with the JSON formatter to serialize objects.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;
using MyApp.Examples;

new JsonFormatterOptionsExample().SerializeWithCustomOptions();

namespace MyApp.Examples
{
    public class JsonFormatterOptionsExample
    {
        public void SerializeWithCustomOptions()
        {
            // Create options with custom JSON serializer settings
            var options = new JsonFormatterOptions
            {
                Settings = new JsonSerializerOptions
                {
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a647487cd2dd0efd2f431360"><a href="~/run-actions/a647487cd2dd0efd2f431360/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a647487cd2dd0efd2f431360/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
