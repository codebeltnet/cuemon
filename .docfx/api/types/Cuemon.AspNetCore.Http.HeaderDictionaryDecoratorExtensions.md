---
uid: Cuemon.AspNetCore.Http.HeaderDictionaryDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the decorator extensions to merge, sanitize, and update HTTP header dictionaries.

```csharp
using System;
using System.Net.Http;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MyApp.AspNetCore.Http
{
    public class HeaderDictionaryDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Add non-existing headers from one dictionary into another
            var target = new HeaderDictionary
            {
                { "X-Existing", "value1" }
            };

            var source = new HeaderDictionary
            {
                { "X-Existing", "value1" },
                { "X-New", "value2" }
            };

            // Only adds headers that do not already exist in target
            Decorator.Enclose<IHeaderDictionary>(target).AddRange(source);

            Console.WriteLine(target["X-Existing"]); // "value1"
            Console.WriteLine(target["X-New"]);      // "value2"

            // Add or update a single header with control-character sanitization
            Decorator.Enclose<IHeaderDictionary>(target).AddOrUpdateHeader(
                "X-Sanitized", new StringValues("hello\r\nworld"), useAsciiEncodingConversion: false);

            Console.WriteLine(target["X-Sanitized"]); // "helloworld"

            // Copy response headers into an IHeaderDictionary
            using var response = new HttpResponseMessage();
            response.Headers.Add("X-Custom", new[] { "alpha", "beta" });

            Decorator.Enclose<IHeaderDictionary>(target).AddOrUpdateHeaders(response.Headers);

            Console.WriteLine(target["X-Custom"]); // "alpha,beta"

}}
}

```
