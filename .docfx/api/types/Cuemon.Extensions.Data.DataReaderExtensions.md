---
uid: Cuemon.Extensions.Data.DataReaderExtensions
example:
- *content
---

The following example demonstrates how to turn a delimiter-separated reader into row and column transfer objects.

```csharp
using System;
using System.IO;
using System.Linq;
using System.Text;
using Cuemon.Data;
using Cuemon.Extensions.Data;

namespace MyApp.Examples;

public static class DataReaderExtensionsExample
{
    public static void Demonstrate()
    {
        var csv = "Id,Name\n1,Alice\n2,Bob";
        using var reader = new DsvDataReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))));

        var rows = reader.ToRows();
        Console.WriteLine(rows.Count);
        Console.WriteLine(rows.ColumnNames.Contains("Name"));

        using var columnReader = new DsvDataReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))));
        columnReader.Read();
        var columns = columnReader.ToColumns();

        Console.WriteLine(columns.Count);
    }
}
```
