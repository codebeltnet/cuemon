---
uid: Cuemon.Extensions.Text.Json.JsonSerializerOptionsExtensions
example:
- *content
---

`JsonSerializerOptionsExtensions` provides `Clone`, `SetPropertyName`, and `DefaultOrConvertName` extension methods for duplicating and transforming `JsonSerializerOptions` instances. This example creates base options with `CamelCase` and `WriteIndented = true`, clones them with `WriteIndented = false` via `Clone`, and demonstrates `SetPropertyName` which converts property names according to the naming policy. Key steps include passing a setup delegate to `Clone` to override specific settings without modifying the original. Console output confirms the original has `WriteIndented = True` while the clone has `False`, and `SetPropertyName("OrderDate")` returns `"orderDate"` under CamelCase or `"OrderDate"` when no policy is set.

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
