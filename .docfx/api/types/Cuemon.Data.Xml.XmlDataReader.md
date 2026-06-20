---
uid: Cuemon.Data.Xml.XmlDataReader
example:
- *content
---

The following example demonstrates how to read XML data as a tabular result set using `XmlDataReader`. It iterates through records, accesses fields by name, and prints row metadata such as depth and field count.

```csharp
using System;
using System.IO;
using System.Xml;
using Cuemon.Data.Xml;

namespace MyApp.Data
{
    public class XmlDataReaderExample
    {
        public void Demonstrate()
        {
            // Create XML data to read
            var xml = @"<records>
                <record><id>1</id><name>Alice</name><score>95.5</score></record>
                <record><id>2</id><name>Bob</name><score>87.0</score></record>
                <record><id>3</id><name>Charlie</name><score>92.3</score></record>
            </records>";

            using var stringReader = new StringReader(xml);
            using var xmlReader = XmlReader.Create(stringReader);

            // Create the XmlDataReader
            using var dataReader = new XmlDataReader(xmlReader);

            // Read through the records like a database result set
            while (dataReader.Read())
            {
                Console.WriteLine($"Row {dataReader.RowCount}:");
                Console.WriteLine($"  Id: {dataReader["id"]}");
                Console.WriteLine($"  Name: {dataReader["name"]}");
                Console.WriteLine($"  Score: {dataReader["score"]}");
                Console.WriteLine($"  Depth: {dataReader.Depth}");
            }

            Console.WriteLine($"Total rows read: {dataReader.RowCount}");

            // Verify field count
            Console.WriteLine($"Fields per row: {dataReader.FieldCount}");
        }
    }
}
```
