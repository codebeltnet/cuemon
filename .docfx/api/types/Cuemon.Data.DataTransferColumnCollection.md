---
uid: Cuemon.Data.DataTransferColumnCollection
example:
- *content
---

The following example demonstrates how to use `DataTransferColumnCollection` to access column metadata retrieved from a data reader, including lookup by name or ordinal.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public sealed class DataTransferColumnCollectionExample
    {
        public void Demonstrate()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Rows.Add(1, "Alice", new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc));

            using var reader = table.CreateDataReader();
            reader.Read();

            DataTransferColumnCollection columns = DataTransfer.GetColumns(reader);

            Console.WriteLine($"Column count: {columns.Count}");
            Console.WriteLine($"First column: {columns[0].Name} ({columns[0].DataType.Name})");
            Console.WriteLine($"Name column ordinal: {columns["Name"].Ordinal}");
            Console.WriteLine($"Missing column found: {columns["Missing"] != null}");
        }
    }
}
```
