---
uid: Cuemon.Data.DsvDataReader
example:
- *content
---

The following example demonstrates how to read a CSV (comma-separated values) file using `DsvDataReader`.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.Data;

namespace MyApp.Examples
{
    public sealed class DsvDataReaderExample
    {
        public void Demonstrate()
        {
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
        }
    }
}
```
