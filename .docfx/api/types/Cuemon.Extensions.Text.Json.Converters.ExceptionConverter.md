---
uid: Cuemon.Extensions.Text.Json.Converters.ExceptionConverter
example:
- *content
---

`ExceptionConverter` serializes `Exception` instances to JSON, including optional stack trace and `Data` dictionary content. This example configures `JsonSerializerOptions` with `WriteIndented = true` and adds the converter with `includeStackTrace: true` and `includeData: true`, then creates a nested `InvalidOperationException("Outer operation failed.")` with an inner `InvalidOperationException("Inner operation failed.")` and a `CorrelationId` data entry. The resulting JSON includes top-level fields (`Type`, `Source`, `Message`, `Stack`, `Data`) and a nested `Inner` section for the inner exception with its own `Type` and `Message`. Console output displays the complete JSON structure.

```csharp
// Program.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json.Converters;

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    WriteIndented = true
};

// Include both stack trace and exception data
options.Converters.Add(new ExceptionConverter(includeStackTrace: true, includeData: true));

var inner = new InvalidOperationException("Inner operation failed.");
InvalidOperationException ex;
try
{
    throw new InvalidOperationException("Outer operation failed.", inner);
}
catch (InvalidOperationException caught)
{
    ex = caught;
    ex.Source = "MyApp";
    ex.Data["CorrelationId"] = "abc-123";
}

string json = JsonSerializer.Serialize(ex, options);
Console.WriteLine(json);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="baa6719012d30b6fda2e6a16"><a href="~/run-actions/baa6719012d30b6fda2e6a16/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/baa6719012d30b6fda2e6a16/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
