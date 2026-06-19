---
uid: Cuemon.Extensions.Text.Json.JsonNamingPolicyExtensions
example:
- *content
---

The following example demonstrates applying a naming policy to a property name using the <xref:Cuemon.Extensions.Text.Json.JsonNamingPolicyExtensions.DefaultOrConvertName(System.Text.Json.JsonNamingPolicy,System.String)> extension method.

```csharp
using System;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace MyApp.Examples;

public class JsonNamingPolicyExtensionsExample
{
    public static void Main()
    {
        // Apply camelCase naming policy
        JsonNamingPolicy camelCase = JsonNamingPolicy.CamelCase;
        string camelName = camelCase.DefaultOrConvertName("OrderDate");
        Console.WriteLine(camelName); // Output: "orderDate"

        // When policy is null, the name is returned unaltered
        string unchanged = ((JsonNamingPolicy)null).DefaultOrConvertName("OrderDate");
        Console.WriteLine(unchanged); // Output: "OrderDate"

        // Useful when working with configurable naming policies
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string configuredName = options.PropertyNamingPolicy.DefaultOrConvertName("ShippingAddress");
        Console.WriteLine(configuredName); // Output: "shippingAddress"

}
}

```
