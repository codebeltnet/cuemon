---
uid: Cuemon.Data.DataTransferColumnCollection
example:
- *content
---

`DataTransferColumnCollection` is a strongly typed collection of `DataTransferColumn` objects that provides access to column metadata by index and by name. This example creates a `DataTable` with `Id`, `Name`, and `Created` columns containing one data row, then retrieves the column collection via `DataTransfer.GetColumns` from the data reader. It accesses the first column by index, looks up the `"Name"` column by name to get its ordinal, and checks whether a missing column name returns `null`. Console output shows the column count, the first column's name and data type, and the missing-column lookup result.

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
