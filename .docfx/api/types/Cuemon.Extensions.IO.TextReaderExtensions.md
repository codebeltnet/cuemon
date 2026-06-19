---
uid: Cuemon.Extensions.IO.TextReaderExtensions
example:
- *content
---

The following example demonstrates how to read lines from a <see cref="TextReader" /> and copy its content asynchronously.

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
