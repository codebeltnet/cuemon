---
uid: Cuemon.DataPair`1
example:
- *content
---

The following example demonstrates how to use the generic <xref cref="Cuemon.DataPair{T}"/> class to represent typed metadata.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;

        // Create a strongly typed DataPair<T> for an integer value
        var age = new DataPair<int>("Age", 30);
        Console.WriteLine(age.HasValue); // True
        Console.WriteLine(age.Name);     // Age
        Console.WriteLine(age.Value);    // 30
        Console.WriteLine(age.Type);     // System.Int32

        // Create a DataPair<T> with a null value
        var middleName = new DataPair<string>("MiddleName", null);
        Console.WriteLine(middleName.HasValue); // False
        Console.WriteLine(middleName);          // Name: MiddleName, Value: <null>, Type: String

        // Override the type metadata (e.g., for a derived type)
        var now = new DataPair<DateTime>("Created", DateTime.UtcNow, typeof(DateTimeOffset));
        Console.WriteLine(now.Type); // System.DateTimeOffset

        // Use DataPair<T> in a collection
        var pairs = new List<DataPair<object>>
        {
            new("Id", Guid.NewGuid()),
            new("Timestamp", DateTime.UtcNow),
            new("Active", true)
        };

        foreach (var pair in pairs)
        {
            Console.WriteLine(pair);

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="518c1f9c16336835d4a1b1db"><a href="~/run-actions/518c1f9c16336835d4a1b1db/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/518c1f9c16336835d4a1b1db/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
