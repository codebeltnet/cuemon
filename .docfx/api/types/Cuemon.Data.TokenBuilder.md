---
uid: Cuemon.Data.TokenBuilder
example:
- *content
---

The following example demonstrates how to use `TokenBuilder` to build delimited token strings with support for quoted fields.

```csharp
using System;
using Cuemon.Data;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        // Build a token string with 4 fields, comma-delimited, double-quote qualified
        var builder = new TokenBuilder(',', '"', 4);

        builder.Append("Alice");
        builder.Append(",30,");
        builder.Append("New York");

        Console.WriteLine($"Is valid: {builder.IsValid}");  // false - only 3 of 4 tokens
        Console.WriteLine($"Current:  '{builder}'");

        // Append the last field to complete the token row
        builder.Append("Engineer");
        Console.WriteLine($"Is valid: {builder.IsValid}");  // true
        Console.WriteLine($"Complete: '{builder}'");

        // TokenBuilder is used internally by DsvDataReader for multi-line quoted fields
        // It accumulates input until the expected number of tokens is reached
        var csvBuilder = new TokenBuilder(';', '\"', 3);
        csvBuilder.Append("Product A");
        csvBuilder.Append("Description with ; semicolon");
        csvBuilder.Append("$49.99");

        Console.WriteLine($"CSV line: '{csvBuilder}'");
        Console.WriteLine($"Tokens:   {csvBuilder.Tokens}");

}
}

```
