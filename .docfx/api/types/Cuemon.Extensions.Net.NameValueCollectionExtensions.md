---
uid: Cuemon.Extensions.Net.NameValueCollectionExtensions
example:
- *content
---

The following example demonstrates how to convert a NameValueCollection into a query string using NameValueCollectionExtensions, with optional URL encoding for safe HTTP transmission.

```csharp
using System;
using System.Collections.Specialized;
using Cuemon.Extensions.Net;

namespace MyApp.Net
{
    public class NameValueCollectionExtensionsExample
    {
        public void Demonstrate()
        {
            var nvc = new NameValueCollection
            {
                { "name", "John Doe" },
                { "city", "Copenhagen" },
                { "hobbies", "reading" },
                { "hobbies", "coding" }
            };

            // Convert NameValueCollection to a query string (not URL-encoded)
            string queryString = nvc.ToQueryString();
            Console.WriteLine(queryString);
            // Output: name=John Doe&city=Copenhagen&hobbies=reading&hobbies=coding

            // URL-encode the values for safe HTTP transmission
            string encoded = nvc.ToQueryString(urlEncode: true);
            Console.WriteLine(encoded);
            // Output: name=John+Doe&city=Copenhagen&hobbies=reading&hobbies=coding

            // Empty collection
            var empty = new NameValueCollection();
            string emptyResult = empty.ToQueryString();
            Console.WriteLine($"Empty: '{emptyResult}'"); // Empty: ''

}}
}

```
