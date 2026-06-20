---
uid: Cuemon.Extensions.IO.TextReaderExtensions
example:
- *content
---

`TextReaderExtensions` provides extension methods for `TextReader` including `ReadAllLines`, `ReadAllLinesAsync`, and `CopyToAsync` for reading and copying text content. This example creates a multi-line string `"line one\nline two\nline three"`, converts it to a `TextReader` via `ToTextReader`, then calls `ReadAllLines` synchronously and `ReadAllLinesAsync` asynchronously to collect each line. It also uses `CopyToAsync` to write the reader content into a `StringWriter`. Console output confirms all three approaches produce the correct line count of `3` and that the copied content contains `"line two"`.

```csharp
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;

namespace MyApp.Examples
{
    public static class TextReaderExtensionsExample
    {
        public static async Task DemonstrateAsync()
        {
            const string input = "line one\nline two\nline three";

            using var linesReader = input.ToTextReader();
            var lines = linesReader.ReadAllLines().ToList();

            using var asyncLinesReader = input.ToTextReader();
            using var writer = new StringWriter();
            var asyncLines = await asyncLinesReader.ReadAllLinesAsync();

            using var copyReader = input.ToTextReader();
            await copyReader.CopyToAsync(writer);

            Console.WriteLine(lines.Count);
            Console.WriteLine(asyncLines.Count);
            Console.WriteLine(writer.ToString().Contains("line two"));
        }
    }
}
```
