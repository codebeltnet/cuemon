---
uid: Cuemon.Data.DataTransfer
example:
- *content
---

The following example demonstrates how to use <see cref="DataTransfer"/> to convert an <see cref="System.Data.IDataReader"/> into row-based and column-based collections.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data;

public sealed class DataTransferExample
{
    public void Demonstrate()
    {
        var table = new DataTable("Products");
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Price", typeof(decimal));
        table.Rows.Add(1, "Widget", 9.99m);
        table.Rows.Add(2, "Gadget", 24.95m);

        using IDataReader reader = table.CreateDataReader();

        // Convert reader rows to a collection
        DataTransferRowCollection rows = DataTransfer.GetRows(reader);
        Console.WriteLine($"Row count: {rows.Count}");
        Console.WriteLine($"Columns: {string.Join(", ", rows.ColumnNames)}");

        // Access data by column name
        foreach (DataTransferRow row in rows)
        {
            Console.WriteLine($"{row["Id"]}: {row["Name"]} @ {row["Price"]:C}");
        }

        // Re-read and get columns
        reader.Dispose();
        using IDataReader reader2 = table.CreateDataReader();
        reader2.Read();
        DataTransferColumnCollection columns = DataTransfer.GetColumns(reader2);
        Console.WriteLine($"Column count: {columns.Count}");
        Console.WriteLine($"First column: {columns[0].Name} ({columns[0].DataType.Name})");
    }
}
```
