---
uid: Cuemon.Net.StringDecoratorExtensions
example:
- *content
---

The following example demonstrates how to URL-encode and URL-decode strings using StringDecoratorExtensions, with support for custom encodings.

```csharp
using System;
using Cuemon;
using System.Text;
using Cuemon.Net;
using Cuemon.Text;

namespace MyApp.Net
{
    public class StringDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // URL-encode a string with special characters
            string raw = "hello world & some <stuff>";

            // Invoke UrlEncode extension method
            string encoded = Decorator.Enclose(raw).UrlEncode();
            Console.WriteLine(encoded); // "hello+world+%26+some+%3cstuff%3e"

            // Also invoke as static method via the Cuemon.Net.StringDecoratorExtensions type
            string encodedStatic = Cuemon.Net.StringDecoratorExtensions.UrlEncode(Decorator.Enclose(raw));
            Console.WriteLine(encodedStatic); // "hello+world+%26+some+%3cstuff%3e"

            // URL-decode the encoded string back
            string decoded = Decorator.Enclose(encoded).UrlDecode();
            Console.WriteLine(decoded); // "hello world & some <stuff>"

            // Invoke UrlDecode as static method via the Cuemon.Net.StringDecoratorExtensions type
            string decodedStatic = Cuemon.Net.StringDecoratorExtensions.UrlDecode(Decorator.Enclose(encoded));
            Console.WriteLine(decodedStatic); // "hello world & some <stuff>"

            // Encode with a specific encoding
            string encodedUtf8 = Decorator.Enclose("a=b&c=d").UrlEncode(o =>
            {
                o.Encoding = Encoding.UTF8;
            });
            Console.WriteLine(encodedUtf8); // "a%3db%26c%3dd"

        }
    }
}

```
