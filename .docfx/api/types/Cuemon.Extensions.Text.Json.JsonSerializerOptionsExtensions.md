---
uid: Cuemon.Extensions.Text.Json.JsonSerializerOptionsExtensions
example:
- *content
---

The following example demonstrates cloning <see cref="JsonSerializerOptions"/> and applying property naming policies using the <xref:Cuemon.Extensions.Text.Json.JsonSerializerOptionsExtensions.Clone(System.Text.Json.JsonSerializerOptions,System.Action{System.Text.Json.JsonSerializerOptions})> and <xref:Cuemon.Extensions.Text.Json.JsonSerializerOptionsExtensions.SetPropertyName(System.Text.Json.JsonSerializerOptions,System.String)> extension methods.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace MyApp.Examples;

public class JsonSerializerOptionsExtensionsExample
{
    public static void Main()
    {
        // Create base options with a camelCase naming policy
        var baseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        // Clone the options and override the WriteIndented setting
        JsonSerializerOptions cloned = baseOptions.Clone(options =>
        {
            options.WriteIndented = false;
        });
        Console.WriteLine($"Original WriteIndented: {baseOptions.WriteIndented}");   // True
        Console.WriteLine($"Cloned WriteIndented: {cloned.WriteIndented}");           // False

        // SetPropertyName converts property names according to the naming policy
        string propertyName = baseOptions.SetPropertyName("OrderDate");
        Console.WriteLine($"Converted property name: {propertyName}"); // Output: "orderDate"

        // When no naming policy is set, the name is returned unaltered
        var plainOptions = new JsonSerializerOptions();
        string unchanged = plainOptions.SetPropertyName("OrderDate");
        Console.WriteLine($"Unchanged property name: {unchanged}"); // Output: "OrderDate"

}
}

```
