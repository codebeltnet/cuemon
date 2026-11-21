---
uid: Cuemon.Extensions.AspNetCore.Text.Json.Converters.JsonConverterCollectionExtensions
example:
- *content
---

The following example demonstrates how to register ASP.NET Core-specific JSON converters using the <xref:Cuemon.Extensions.AspNetCore.Text.Json.Converters.JsonConverterCollectionExtensions> class.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

var converters = new List<JsonConverter>();

// Add converter for IHeaderDictionary
converters.AddHeaderDictionaryConverter();

// Add converter for ProblemDetails
converters.AddProblemDetailsConverter();

// Add converter for HttpExceptionDescriptor
converters.AddHttpExceptionDescriptorConverter();

// Add converter for StringValues
converters.AddStringValuesConverter();

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
};
foreach (var converter in converters)
{
    options.Converters.Add(converter);
}

// Example: serialize a HeaderDictionary
var headers = new HeaderDictionary
{
    { "X-Custom", "value1" },
    { "X-Another", "value2" }
};

string json = JsonSerializer.Serialize(headers, options);
Console.WriteLine(json);
// Output: {"X-Custom":"value1","X-Another":"value2"}

// Example: serialize StringValues
var values = new StringValues(new[] { "a", "b", "c" });
string svJson = JsonSerializer.Serialize(values, options);
Console.WriteLine(svJson);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a5f97890f767889de61c5c91"><a href="~/run-actions/a5f97890f767889de61c5c91/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a5f97890f767889de61c5c91/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
