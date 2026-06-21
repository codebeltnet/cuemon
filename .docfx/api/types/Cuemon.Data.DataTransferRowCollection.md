---
uid: Cuemon.Data.DataTransferRowCollection
example:
- *content
---

The following example demonstrates how to use <see cref="DataTransferRowCollection"/> to work with rows returned from a database query via an <see cref="System.Data.IDataReader"/>.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public sealed class DataTransferRowCollectionExample
    {
        public void Demonstrate()
        {
            var table = new DataTable("Products");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Columns.Add("Notes", typeof(string));
            table.Rows.Add(1, "Apples", new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), "Fresh");
            table.Rows.Add(2, "Bananas", new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Utc), DBNull.Value);

            using var reader = table.CreateDataReader();
            DataTransferRowCollection rows = DataTransfer.GetRows(reader);

            Console.WriteLine("Columns: " + string.Join(", ", rows.ColumnNames));
            Console.WriteLine($"Row count: {rows.Count}");

            DataTransferRow firstRow = rows[0];
            Console.WriteLine(firstRow.ToString());
            Console.WriteLine($"First row name: {firstRow["Name"]}");
            Console.WriteLine($"Created: {firstRow.As<DateTime>("Created"):O}");

            DataTransferRow secondRow = rows[1];
            Console.WriteLine($"Second row notes are null: {secondRow["Notes"] == null}");
            Console.WriteLine($"Contains first row: {rows.Contains(firstRow)}");
            Console.WriteLine($"Index of first row: {rows.IndexOf(firstRow)}");
        }
    }
}
```
