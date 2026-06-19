---
uid: Cuemon.Extensions.Collections.Specialized.DictionaryExtensions
example:
- *content
---

The following example demonstrates how to convert a Dictionary with string array values into a NameValueCollection using DictionaryExtensions, with support for custom delimiters.

```csharp
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon;
using Cuemon.Extensions.Collections.Specialized;

namespace MyApp.Extensions.Collections.Specialized
{
    public class DictionaryExtensionsExample
    {
        public void Demonstrate()
        {
            // Create a dictionary with string array values
            var source = new Dictionary<string, string[]>
            {
                ["colors"] = new[] { "red", "green", "blue" },
                ["sizes"] = new[] { "small", "large" }
            };

            // Convert to a NameValueCollection (default delimiter is comma)
            NameValueCollection nvc = source.ToNameValueCollection();

            Console.WriteLine(nvc["colors"]); // "red,green,blue"
            Console.WriteLine(nvc["sizes"]);  // "small,large"

            // Convert with a custom delimiter
            NameValueCollection nvcSemicolon = source.ToNameValueCollection(o =>
            {
                o.Delimiter = ";";
            });

            Console.WriteLine(nvcSemicolon["colors"]); // "red;green;blue"
            Console.WriteLine(nvcSemicolon["sizes"]);  // "small;large"

}}
}

```
