---
uid: Cuemon.Data.DataTransferColumn
example:
- *content
---

The following example demonstrates how to use `DataTransferColumn` to inspect column metadata — name, ordinal, and data type — from a data reader.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public class DataTransferColumnExample
    {
        public void Demonstrate()
        {
            // Create a DataTable as a sample data source
            var table = new DataTable("Employees");
            table.Columns.Add("EmployeeId", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("HireDate", typeof(System.DateTime));

            table.Rows.Add(1, "John", "Doe", new DateTime(2023, 6, 1));
            table.Rows.Add(2, "Jane", "Smith", new DateTime(2024, 1, 15));

            // Obtain a DataTransferColumnCollection from an IDataReader via DataTransfer
            using var reader = table.CreateDataReader();
            var columns = DataTransfer.GetColumns(reader);

            Console.WriteLine($"Columns ({columns.Count}):");
            foreach (DataTransferColumn column in columns)
            {
                Console.WriteLine($"  [{column.Ordinal}] {column.Name} ({column.DataType.Name})");

            // Output:
            //   [0] EmployeeId (Int32)
            //   [1] FirstName (String)
            //   [2] LastName (String)
            //   [3] HireDate (DateTime)

            // Access columns by name
            DataTransferColumn firstNameCol = columns["FirstName"];
            Console.WriteLine($"Ordinal of 'FirstName': {firstNameCol.Ordinal}"); // 1

            // ToString() returns the column name
            Console.WriteLine($"Column ToString: {firstNameCol}"); // FirstName

}}}
}

```
