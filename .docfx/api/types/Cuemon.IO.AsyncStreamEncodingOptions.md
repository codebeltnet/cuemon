---
uid: Cuemon.IO.AsyncStreamEncodingOptions
example:
- *content
---

`AsyncStreamEncodingOptions` configures encoding, preamble handling, and stream lifetime when reading text from a stream asynchronously. This example creates options with `Encoding.UTF8`, `PreambleSequence.Remove` to strip the BOM from output, and `LeaveOpen = false`, then creates a `MemoryStream` with UTF-8 encoded text and reads the content using a `StreamReader` with the configured encoding. Console output displays `"Hello, AsyncStreamEncodingOptions!"`, confirming the text was read correctly with the configured encoding settings.

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
