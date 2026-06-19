---
uid: Cuemon.Extensions.Net.DictionaryExtensions
example:
- *content
---

The following example demonstrates how to build a query string from a dictionary of string arrays using DictionaryExtensions, with optional URL encoding.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Net;

namespace MyApp.Net
{
    public class DictionaryExtensionsExample
    {
        public void Demonstrate()
        {
            // Build a query string from a dictionary
            var parameters = new Dictionary<string, string[]>
            {
                { "search", new[] { "dotnet" } },
                { "page", new[] { "1" } },
                { "tags", new[] { "aspnet", "core" } }
            };

            // Convert to a query string (not URL-encoded)
            string queryString = parameters.ToQueryString();
            Console.WriteLine(queryString);
            // Output: search=dotnet&page=1&tags=aspnet&tags=core

            // URL-encode the values
            string encoded = parameters.ToQueryString(urlEncode: true);
            Console.WriteLine(encoded);
            // Output: search=dotnet&page=1&tags=aspnet&tags=core

            // Useful for building API request URLs
            var empty = new Dictionary<string, string[]>();
            string emptyQs = empty.ToQueryString();
            Console.WriteLine($"Empty: '{emptyQs}'"); // Empty: ''

}}
}

```
