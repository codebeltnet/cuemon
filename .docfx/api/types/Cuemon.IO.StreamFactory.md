---
uid: Cuemon.IO.StreamFactory
example:
- *content
---

The following example demonstrates how to create <see cref="System.IO.Stream"/> instances using <see cref="StreamFactory"/>.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.IO;
using Cuemon.Text;

namespace MyApp.Examples;

public static class StreamFactoryExample
{
    public static void Demonstrate()
    {
        // Create a stream by writing to a StreamWriter
        Stream stream = StreamFactory.Create(writer =>
        {
            writer.Write("Hello, StreamFactory!");
        });

        Console.WriteLine($"Stream length: {stream.Length}");
        Console.WriteLine($"Stream position: {stream.Position}");

        using var reader = new StreamReader(stream);
        string content = reader.ReadToEnd();
        Console.WriteLine(content);

        // Create a stream with encoding options
        Stream utf32Stream = StreamFactory.Create(writer =>
        {
            writer.Write("UTF-32 encoded content");
        }, options =>
        {
            options.Encoding = Encoding.UTF32;
            options.Preamble = PreambleSequence.Remove;
        });

        using var utf32Reader = new StreamReader(utf32Stream, Encoding.UTF32);
        Console.WriteLine(utf32Reader.ReadToEnd());
    }
}
```
