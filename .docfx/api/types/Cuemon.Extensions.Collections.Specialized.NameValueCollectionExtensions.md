---
uid: Cuemon.Extensions.Collections.Specialized.NameValueCollectionExtensions
example:
- *content
---

The following example demonstrates how to use NameValueCollectionExtensions to check for key existence and convert a NameValueCollection into a dictionary with string array values.

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
