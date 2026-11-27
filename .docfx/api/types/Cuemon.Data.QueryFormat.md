---
uid: Cuemon.Data.QueryFormat
example:
- *content
---

`QueryFormat` is an enumeration that controls how `QueryBuilder.EncodeFragment` formats column and value fragments for SQL statement generation. This example calls `EncodeFragment` with `QueryFormat.Delimited` on column names (`"FirstName,LastName,Email"`), `DelimitedString` on string values (`"'John','Doe'"`), and `DelimitedSquareBracket` on identifiers (`"[FirstName],[LastName]"`). It also demonstrates the `distinct: true` option that removes duplicate values from the output. Console output shows each formatting style applied to the sample data, such as `FirstName,LastName,Email` for delimited and `[FirstName],[LastName]` for square-bracket delimited.

```csharp
// Program.cs
using System;
using Cuemon.Data;

// QueryFormat controls how query fragments are formatted.

// Delimited: value, value, value
string delimited = QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "FirstName", "LastName", "Email" });
Console.WriteLine($"Delimited: {delimited}"); // FirstName,LastName,Email

// DelimitedString: 'value', 'value', 'value'
string delimitedString = QueryBuilder.EncodeFragment(QueryFormat.DelimitedString, new[] { "John", "Doe" });
Console.WriteLine($"DelimitedString: {delimitedString}"); // 'John','Doe'

// DelimitedSquareBracket: [value], [value], [value]
string delimitedSquareBracket = QueryBuilder.EncodeFragment(QueryFormat.DelimitedSquareBracket, new[] { "FirstName", "LastName" });
Console.WriteLine($"DelimitedSquareBracket: {delimitedSquareBracket}"); // [FirstName],[LastName]

// Distinct option removes duplicates
string distinct = QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "A", "B", "A", "C" }, distinct: true);
Console.WriteLine($"Distinct: {distinct}"); // A,B,C
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b2d4656bd77614b4388440ff"><a href="~/run-actions/b2d4656bd77614b4388440ff/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b2d4656bd77614b4388440ff/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
