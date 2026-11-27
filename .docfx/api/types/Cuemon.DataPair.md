---
uid: Cuemon.DataPair
example:
- *content
---

The following example demonstrates how to use the <see cref="DataPair"/> and <see cref="DataPair{T}"/> classes to represent named metadata with type information.

```csharp
// Program.cs
using System;
using Cuemon;

// Create a generic DataPair<T> for compile-time type safety
var pair = new DataPair<int>("Age", 30);
Console.WriteLine(pair);
// Output: Name: Age, Value: 30, Type: Int32

Console.WriteLine(pair.Name);   // Age
Console.WriteLine(pair.Value);  // 30
Console.WriteLine(pair.Type);   // System.Int32
Console.WriteLine(pair.HasValue); // True

// Create a non-generic DataPair
var generic = new DataPair("CreatedAt", DateTime.UtcNow, typeof(DateTime));
Console.WriteLine(generic);
// Output: Name: CreatedAt, Value: ..., Type: DateTime

// DataPair with null value
var nullPair = new DataPair<string>("MiddleName", null);
Console.WriteLine(nullPair.HasValue); // False
Console.WriteLine(nullPair);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ef06b063c666179616129890"><a href="~/run-actions/ef06b063c666179616129890/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ef06b063c666179616129890/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
