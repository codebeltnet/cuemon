---
uid: Cuemon.Extensions.IO.StringExtensions
example:
- *content
---

The following example demonstrates how to convert a string into a stream or text reader with <see cref="StringExtensions" />.

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;

namespace MyApp.Examples
{
    public static class StringExtensionsExample
    {
        public static async Task DemonstrateAsync()
        {
            const string json = "{\"key\":\"value\"}";

            using Stream stream = json.ToStream(options =>
            {
                options.Encoding = Encoding.UTF8;
            });

            using Stream asyncStream = await json.ToStreamAsync();
            using TextReader reader = json.ToTextReader();

            Console.WriteLine(stream.Length > 0);
            Console.WriteLine((await asyncStream.ToEncodedStringAsync()) == json);
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}
```
