---
uid: Cuemon.Net.QueryStringCollection
example:
- *content
---

The following example demonstrates how to create, parse, and manipulate URI query string parameters using QueryStringCollection, with support for URL decoding and cloning.

```csharp
using System;
using System.Linq;
using Cuemon.Net;

namespace MyApp.Net
{
    public static class QueryStringCollectionExamples
    {
        public static void Demonstrate()
        {
            // Create an empty query string collection and add parameters.
            var qsc = new QueryStringCollection();
            qsc.Add("search", "dotnet");
            qsc.Add("page", "2");
            qsc.Add("sort", "name");
            Console.WriteLine("Query string: {0}", qsc); // search=dotnet&page=2&sort=name

            // Create from an existing URI query string.
            var fromUrl = new QueryStringCollection("?category=books&author=tolkien");
            Console.WriteLine("Parsed query: {0}", fromUrl); // category=books&author=tolkien

            // Create with URL decoding enabled.
            var encoded = new QueryStringCollection("q=hello%20world&lang=en", urlDecode: true);
            Console.WriteLine("Decoded 'q': {0}", encoded["q"]); // hello world

            // Iterate over key-value pairs.
            Console.WriteLine("Parameters:");
            foreach (var pair in fromUrl)
            {
                Console.WriteLine("  {0} = {1}", pair.Key, pair.Value);

            // Use AllKeys from the base NameValueCollection.
            Console.WriteLine("Keys: {0}", string.Join(", ", qsc.AllKeys));

            // Clone a QueryStringCollection.
            var clone = new QueryStringCollection(qsc);
            clone["page"] = "3";
            Console.WriteLine("Original page: {0}", qsc["page"]);  // 2
            Console.WriteLine("Cloned page:   {0}", clone["page"]); // 3

            // Count entries.
            Console.WriteLine("Count: {0}", qsc.Count);

}}}
}

```
