---
uid: Cuemon.CharDecoratorExtensions
example:
- *content
---

The following example shows how to extend `IEnumerable<char>` with `CharDecoratorExtensions` methods to convert character sequences into single-character strings and back to a combined string.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;

namespace MyApp
{
    public class CharDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Convert a sequence of characters to a sequence of single-character strings
            IEnumerable<char> characters = "Hello".AsEnumerable();

            IEnumerable<string> strings = Decorator.Enclose(characters).ToEnumerable();
            Console.WriteLine(string.Join(", ", strings)); // "H, e, l, l, o"

            // Convert a sequence of characters back to a single string
            string result = Decorator.Enclose(characters).ToStringEquivalent();
            Console.WriteLine(result); // "Hello"

            // Works with any IEnumerable<char> including char arrays
            char[] charArray = { 'A', 'B', 'C' };
            string joined = Decorator.Enclose(charArray.AsEnumerable()).ToStringEquivalent();
            Console.WriteLine(joined); // "ABC"

}}
}

```
