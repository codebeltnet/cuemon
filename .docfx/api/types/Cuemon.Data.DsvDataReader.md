---
uid: Cuemon.Data.DsvDataReader
example:
- *content
---

The following example demonstrates how to read a CSV (comma-separated values) file using `DsvDataReader`.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using Cuemon.Data;

var csv = "Name,Age,City" + Environment.NewLine +
          "Alice,30,New York" + Environment.NewLine +
          "Bob,25,London" + Environment.NewLine +
          "Charlie,35,Tokyo";

using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
using var reader = new DsvDataReader(new StreamReader(stream));

Console.WriteLine($"Delimiter: {reader.Delimiter}");
Console.WriteLine("Header: " + string.Join(", ", reader.Header));

while (reader.Read())
{
    Console.WriteLine($"Row {reader.RowCount}: Name={reader["Name"]}, Age={reader["Age"]}, City={reader["City"]}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bdc2c0e82941d0102b6f6846"><a href="~/run-actions/bdc2c0e82941d0102b6f6846/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bdc2c0e82941d0102b6f6846/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
