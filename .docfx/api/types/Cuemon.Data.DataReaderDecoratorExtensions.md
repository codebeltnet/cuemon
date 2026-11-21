---
uid: Cuemon.Data.DataReaderDecoratorExtensions
example:
- *content
---

`DataReaderDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting `IDataReader` content into encoded strings or streams, both synchronously and asynchronously. This example creates a single-column `DataTable` with three rows of data, then wraps a fresh `DataTableReader` for each call to `ToEncodedString`, `ToEncodedStringAsync`, and `ToStream`. Console output shows the string representation of the data and the stream length.

```csharp
// Program.cs
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Data;

var table = new DataTable();
table.Columns.Add("Value", typeof(string));
table.Rows.Add("1001");
table.Rows.Add("1002");
table.Rows.Add("1003");

using (var reader = table.CreateDataReader())
{
    string result = Decorator.Enclose((IDataReader)reader).ToEncodedString();
    Console.WriteLine(result);
}

using (var asyncReader = table.CreateDataReader())
{
    string asyncResult = await Decorator.Enclose((IDataReader)asyncReader).ToEncodedStringAsync();
    Console.WriteLine(asyncResult);
}

using (var streamReader = table.CreateDataReader())
{
    Stream dataStream = Decorator.Enclose((IDataReader)streamReader).ToStream();
    Console.WriteLine($"Stream length: {dataStream.Length}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e8877b9842aaa91c8e5bbfa0"><a href="~/run-actions/e8877b9842aaa91c8e5bbfa0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e8877b9842aaa91c8e5bbfa0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
