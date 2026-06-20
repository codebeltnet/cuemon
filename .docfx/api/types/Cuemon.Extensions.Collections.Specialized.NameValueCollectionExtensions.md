---
uid: Cuemon.Extensions.Collections.Specialized.NameValueCollectionExtensions
example:
- *content
---

`NameValueCollectionExtensions` provides extension methods for `NameValueCollection` including case-insensitive key lookup and conversion to `IDictionary<string, string[]>`. This example creates a collection with `"name": "John Doe"` and `"tag": ["dotnet", "csharp"]` (duplicate key), then calls `ContainsKey("NAME")` to verify case-insensitive matching and `ToDictionary()` to materialize entries as a dictionary with string array values. It also demonstrates `ToDictionary` with a custom `";"` delimiter for splitting multi-valued entries. Console output confirms boolean key-lookup results, the individual array elements `"dotnet"` and `"csharp"`, and the array length when using the custom delimiter.

```csharp
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon;
using Cuemon.Extensions.Collections.Specialized;

namespace MyApp.Extensions.Collections.Specialized
{
    public class NameValueCollectionExtensionsExample
    {
        public void Demonstrate()
        {
            // Create a NameValueCollection with some query parameters
            var nvc = new NameValueCollection
            {
                { "name", "John Doe" },
                { "tag", "dotnet" },
                { "tag", "csharp" }
            };

            // Check if a key exists (case-insensitive)
            bool hasName = nvc.ContainsKey("NAME");
            Console.WriteLine(hasName); // True

            bool hasMissing = nvc.ContainsKey("missing");
            Console.WriteLine(hasMissing); // False

            // Convert to a dictionary with string[] values
            IDictionary<string, string[]> dict = nvc.ToDictionary();

            Console.WriteLine(dict["name"][0]);   // "John Doe"
            Console.WriteLine(dict["tag"][0]);    // "dotnet"
            Console.WriteLine(dict["tag"][1]);    // "csharp"

            // Use a custom delimiter for splitting values
            var nvcSemicolon = new NameValueCollection
            {
                { "items", "a;b;c" }
            };

            IDictionary<string, string[]> dictSemicolon = nvcSemicolon.ToDictionary(o =>
            {
                o.Delimiter = ";";
            });

            Console.WriteLine(dictSemicolon["items"].Length); // 3

}}
}

```
