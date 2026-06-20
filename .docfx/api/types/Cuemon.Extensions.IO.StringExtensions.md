---
uid: Cuemon.Extensions.IO.StringExtensions
example:
- *content
---

`StringExtensions` in the `IO` namespace converts strings into `Stream` and `TextReader` instances for stream-based processing. This example starts with a JSON string `{"key":"value"}`, calls `ToStream` with a UTF-8 encoding configuration, `ToStreamAsync` for the asynchronous variant, and `ToTextReader` for direct text reader access. Key steps include verifying the stream has positive length, reading back the async stream's content with `ToEncodedStringAsync`, and reading the text reader content with `ReadToEnd`. Console output confirms the stream length is greater than zero, the async round-trip matches the original JSON, and the text reader reads the expected content.

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
