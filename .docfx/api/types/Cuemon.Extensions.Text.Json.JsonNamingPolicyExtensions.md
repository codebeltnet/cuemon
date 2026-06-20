---
uid: Cuemon.Extensions.Text.Json.JsonNamingPolicyExtensions
example:
- *content
---

`JsonNamingPolicyExtensions` provides the `DefaultOrConvertName` extension method that applies a `JsonNamingPolicy` to a property name or returns it unchanged when the policy is `null`. This example applies `JsonNamingPolicy.CamelCase` to `"OrderDate"`, producing `"orderDate"`, and demonstrates that a `null` policy returns the name unaltered. Key steps include calling `DefaultOrConvertName` directly on a policy instance and using it within a `JsonSerializerOptions` configuration. Console output confirms `"orderDate"` for the camelCase transformation and `"OrderDate"` for the null policy case, and `"shippingAddress"` when sourced from serializer options.

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
