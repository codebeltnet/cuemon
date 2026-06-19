---
uid: Cuemon.Data.QueryFormat
example:
- *content
---

The following example demonstrates how to use the `QueryFormat` enumeration to control how query fragments — including delimited, quoted, and bracketed formats — are generated when building SQL queries.

```csharp
using System;
using Cuemon.Data;

namespace MyApp.Data
{
    public class QueryFormatExample
    {
        public void Demonstrate()
        {
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

}}
}

```
