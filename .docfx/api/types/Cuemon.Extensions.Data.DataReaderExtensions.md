---
uid: Cuemon.Extensions.Data.DataReaderExtensions
example:
- *content
---

The following example demonstrates how to turn a delimiter-separated reader into row and column transfer objects.

```csharp
// Program.cs
using System;
using System.IO;
using System.Linq;
using System.Text;
using Cuemon.Data;
using Cuemon.Extensions.Data;

var csv = "Id,Name\n1,Alice\n2,Bob";
using var reader = new DsvDataReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))));

var rows = reader.ToRows();
Console.WriteLine(rows.Count);
Console.WriteLine(rows.ColumnNames.Contains("Name"));

using var columnReader = new DsvDataReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))));
columnReader.Read();
var columns = columnReader.ToColumns();

Console.WriteLine(columns.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0eefa623c34dab3154b9615c"><a href="~/run-actions/0eefa623c34dab3154b9615c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0eefa623c34dab3154b9615c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
