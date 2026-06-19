---
uid: Cuemon.IO.AsyncStreamEncodingOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncStreamEncodingOptions` when reading text from a stream asynchronously.

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Text;

namespace MyApp.Examples;

public class AsyncStreamEncodingOptionsExample
{
    public static async Task Main()
    {
        var options = new AsyncStreamEncodingOptions
        {
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Remove,
            LeaveOpen = false
        };

        string text = "Hello, AsyncStreamEncodingOptions!";
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream, options.Encoding);

        string result = await reader.ReadToEndAsync();
        Console.WriteLine(result);

        // Output:
        // Hello, AsyncStreamEncodingOptions!

}
}

```
