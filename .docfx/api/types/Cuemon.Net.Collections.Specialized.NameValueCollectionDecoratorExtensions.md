---
uid: Cuemon.Net.Collections.Specialized.NameValueCollectionDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the `ToString` extension method to convert a `NameValueCollection` into a URI query string.

```csharp
using System;
using System.Collections.Specialized;
using Cuemon;
using Cuemon.Net;
using Cuemon.Net.Collections.Specialized;

namespace MyApp.Examples;

public class NameValueCollectionDecoratorExtensionsExample
{
    public static void Main()
    {
        var nvc = new NameValueCollection
        {
            ["name"] = "John Doe",
            ["city"] = "Copenhagen",
            ["country"] = "Denmark"
        };

        // Convert to URL query string with ampersand separator and URL encoding.
        string queryString = Decorator.Enclose(nvc).ToString(FieldValueSeparator.Ampersand, urlEncode: true);
        Console.WriteLine(queryString);

        // Output:
        // ?name=John%20Doe&city=Copenhagen&country=Denmark

}
}

```
