---
uid: Cuemon.Extensions.Data.QueryFormatExtensions
example:
- *content
---

The following example demonstrates generating query fragments for SQL IN clauses using the [Embed](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.Data.QueryFormatExtensions.html#Cuemon_Extensions_Data_QueryFormatExtensions_Embed_Cuemon_Data_QueryFormat_System_Collections_Generic_IEnumerable_System_String__System_Boolean_) extension method.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Data;
using Cuemon.Extensions.Data;

namespace MyApp.Examples;

public class QueryFormatExtensionsExample
{
    public static void Main()
    {
        // Embed string values in a delimited format: value, value, value
        var productIds = new[] { "ALFKI", "BONAP", "FRANS" };
        string delimited = QueryFormat.Delimited.Embed(productIds);
        Console.WriteLine(delimited); // Output: "ALFKI", "BONAP", "FRANS"

        // Embed string values with single quotes: 'value', 'value', 'value'
        string delimitedString = QueryFormat.DelimitedString.Embed(productIds);
        Console.WriteLine(delimitedString); // Output: 'ALFKI', 'BONAP', 'FRANS'

        // Embed integer values
        var ids = new[] { 1, 2, 3, 4, 5 };
        string intEmbedded = QueryFormat.Delimited.Embed(ids);
        Console.WriteLine(intEmbedded); // Output: 1, 2, 3, 4, 5

        // Embed with distinct filtering to remove duplicates
        var withDuplicates = new[] { "apple", "banana", "apple", "cherry" };
        string distinctFragment = QueryFormat.DelimitedString.Embed(withDuplicates, distinct: true);
        Console.WriteLine(distinctFragment); // Output: 'apple', 'banana', 'cherry'

}
}

```
