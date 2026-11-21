---
uid: Cuemon.Data.TokenBuilder
example:
- *content
---

The following example demonstrates how to use `TokenBuilder` to build delimited token strings with support for quoted fields.

```csharp
// Program.cs
using System;
using Cuemon.Data;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="37f40b9fa3c262d6ee82d474"><a href="~/run-actions/37f40b9fa3c262d6ee82d474/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/37f40b9fa3c262d6ee82d474/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
