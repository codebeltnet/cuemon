---
uid: Cuemon.Data.DataTransferRow
example:
- *content
---

`DataTransferRow` provides access to data reader field values by index, column name, or `DataTransferColumn` object, including type-safe access via generic methods and automatic `DBNull` to `null` conversion. This example creates a `DataTable` with `Id`, `Name`, `Created`, and `Notes` columns containing two rows (one with a `DBNull` note), then retrieves a `DataTransferRowCollection` via `DataTransfer.GetRows`. It accesses values using all three lookup approaches — `first[0]` by index, `first["Name"]` by column name, and `first[idCol]` by column object — and uses `As<int>("Id")` for type-safe access. Console output confirms each value, shows `null` for the `DBNull` case, and displays the string representation of the row.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public static class DataTransferRowExamples
    {
        public static void Demonstrate()
        {
            // Build a DataTable to simulate a database result set.
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Columns.Add("Notes", typeof(string));
            table.Rows.Add(1, "Alice", new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), "First");
            table.Rows.Add(2, "Bob", new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Utc), DBNull.Value);

            // Convert the reader to rows.
            using (IDataReader reader = table.CreateDataReader())
            {
                DataTransferRowCollection rows = DataTransfer.GetRows(reader);

                // Access individual rows.
                DataTransferRow first = rows[0];
                DataTransferRow second = rows[1];

                Console.WriteLine("Row numbers: {0}, {1}", first.Number, second.Number);

                // Access values by column index.
                object idValue = first[0];
                Console.WriteLine("First row Id: {0}", idValue);

                // Access values by column name.
                object nameValue = first["Name"];
                Console.WriteLine("First row Name: {0}", nameValue);

                // Access values by DataTransferColumn object.
                DataTransferColumn idCol = first.Columns["Id"];
                object viaColumn = first[idCol];
                Console.WriteLine("First row Id (via column): {0}", viaColumn);

                // Type-safe access using generics.
                int idTyped = first.As<int>("Id");
                DateTime created = first.As<DateTime>("Created");
                Console.WriteLine("Typed: Id={0}, Created={1:O}", idTyped, created);

                // Nullable value (DBNull is converted to null).
                object notes = second["Notes"];
                Console.WriteLine("Second row Notes is null: {0}", notes == null);

                // String representation of the row.
                Console.WriteLine("Row string: {0}", first);

}}}
}

```
