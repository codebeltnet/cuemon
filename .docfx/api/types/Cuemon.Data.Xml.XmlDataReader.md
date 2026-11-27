---
uid: Cuemon.Data.Xml.XmlDataReader
example:
- *content
---

The following example demonstrates how to read XML data as a tabular result set using `XmlDataReader`. It iterates through records, accesses fields by name, and prints row metadata such as depth and field count.

```csharp
// Program.cs
using System;
using System.IO;
using System.Xml;
using Cuemon.Data.Xml;

// Create XML data to read (attributes on each record element form one tabular row)
var xml = @"<records>
    <record id=""1"" name=""Alice"" score=""95.5"" />
    <record id=""2"" name=""Bob"" score=""87.0"" />
    <record id=""3"" name=""Charlie"" score=""92.3"" />
</records>";

using var stringReader = new StringReader(xml);
using var xmlReader = XmlReader.Create(stringReader);

// Create the XmlDataReader
using var dataReader = new XmlDataReader(xmlReader);

int fieldCount = 0;

// Read through the records like a database result set
while (dataReader.Read())
{
    fieldCount = dataReader.FieldCount;
    Console.WriteLine($"Row {dataReader.RowCount}:");
    Console.WriteLine($"  Id: {dataReader["id"]}");
    Console.WriteLine($"  Name: {dataReader["name"]}");
    Console.WriteLine($"  Score: {dataReader["score"]}");
    Console.WriteLine($"  Depth: {dataReader.Depth}");
}

Console.WriteLine($"Total rows read: {dataReader.RowCount}");

// Verify field count captured while reading rows
Console.WriteLine($"Fields per row: {fieldCount}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="957d8a55fb0f4ec42b021164"><a href="~/run-actions/957d8a55fb0f4ec42b021164/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/957d8a55fb0f4ec42b021164/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
