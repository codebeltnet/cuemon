---
uid: Cuemon.Extensions.Text.Json.Converters.StringFlagsEnumConverter
example:
- *content
---

`StringFlagsEnumConverter` serializes and deserializes `[Flags]` enum values as an array of active flag string names instead of a single integer. This example registers the converter in `JsonSerializerOptions` with `CamelCase` naming policy, then serializes `FileAttributes.ReadOnly | FileAttributes.Hidden` which produces the JSON array `["readOnly","hidden"]`. Deserializing the array back to a `FileAttributes` value restores the combined flags, and the output displays `ReadOnly, Hidden`, confirming round-trip correctness for flags enum values.

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

var value = FileAttributes.ReadOnly | FileAttributes.Hidden;

string json = JsonSerializer.Serialize(value, options);
Console.WriteLine(json);
// ["readOnly","hidden"]

var restored = JsonSerializer.Deserialize<FileAttributes>(json, options);
Console.WriteLine(restored); // ReadOnly, Hidden
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f6a162393281c2f5caa60023"><a href="~/run-actions/f6a162393281c2f5caa60023/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f6a162393281c2f5caa60023/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
