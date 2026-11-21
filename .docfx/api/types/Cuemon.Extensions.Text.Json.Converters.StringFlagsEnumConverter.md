---
uid: Cuemon.Extensions.Text.Json.Converters.StringFlagsEnumConverter
example:
- *content
---

`StringFlagsEnumConverter` serializes and deserializes `[Flags]` enum values as an array of active flag string names instead of a single integer. This example registers the converter in `JsonSerializerOptions`, then serializes `FileShare.Read | FileShare.Write` which produces the JSON array `["Read", "Write"]`. Deserializing the array back to a `FileShare` value restores the combined flags, and the output displays `Read, Write`, confirming round-trip correctness for flags enum values.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Cuemon.Extensions.Text.Json.Converters;

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
options.Converters.Add(new StringFlagsEnumConverter());

var value = FileShare.Read | FileShare.Write;

string json = JsonSerializer.Serialize(value, options);
Console.WriteLine(json);
// ["Read", "Write"]

var restored = JsonSerializer.Deserialize<FileShare>(json, options);
Console.WriteLine(restored); // Read, Write
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="31b8c5f63efa314e3e1981b1"><a href="~/run-actions/31b8c5f63efa314e3e1981b1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/31b8c5f63efa314e3e1981b1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
