---
uid: Cuemon.IO.TextReaderDecoratorExtensions
example:
- *content
---

`TextReaderDecoratorExtensions` provides extension methods on `Decorator.Enclose` for copying `TextReader` content to `TextWriter` instances asynchronously with configurable buffer sizes. This example creates a `StringReader` with three lines of text, copies to a `StringWriter` via `CopyToAsync`, and outputs the result. It also demonstrates a custom buffer size of `1024` for the copy operation and copying between different reader/writer types (`StreamReader` to `StringWriter`). Console output confirms the copied content matches the source for all three scenarios.

```csharp
using System.Text;
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;

namespace MyApp.IO
{
    public class TextReaderDecoratorExtensionsExample
    {
        public async Task DemonstrateAsync()
        {
            string source = "Line 1\nLine 2\nLine 3\n";

            // Copy content from a TextReader to a TextWriter asynchronously
            using var reader = new StringReader(source);
            using var writer = new StringWriter();

            await Decorator.Enclose(reader).CopyToAsync(writer);

            string result = writer.ToString();
            Console.WriteLine(result); // "Line 1\nLine 2\nLine 3\n"

            // Use a custom buffer size
            using var readerSmallBuffer = new StringReader(source);
            using var writerSmallBuffer = new StringWriter();

            await Decorator.Enclose(readerSmallBuffer).CopyToAsync(writerSmallBuffer, bufferSize: 1024);

            string resultSmallBuffer = writerSmallBuffer.ToString();
            Console.WriteLine(resultSmallBuffer); // "Line 1\nLine 2\nLine 3\n"

            // Copy between different TextReader/TextWriter types
            using var streamReader = new StreamReader(new MemoryStream(
                Encoding.UTF8.GetBytes("Stream content")));
            using var stringWriter = new StringWriter();

            await Decorator.Enclose(streamReader).CopyToAsync(stringWriter);
            Console.WriteLine(stringWriter.ToString()); // "Stream content"

}}
}

```
