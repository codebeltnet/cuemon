---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationInputFormatter
example:
- *content
---

The following example demonstrates how to construct a <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationInputFormatter"/> and inspect its supported media types and encodings.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;

namespace DocfxExamples;

public class JsonSerializationInputFormatterExample
{
    public void Demonstrate()
    {
        var options = new JsonFormatterOptions();
        var formatter = new JsonSerializationInputFormatter(options);

        Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
        Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");

}
}

```
