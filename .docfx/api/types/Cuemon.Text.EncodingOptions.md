---
uid: Cuemon.Text.EncodingOptions
example:
- *content
---

The following example demonstrates how to use <see cref="EncodingOptions"/> to control encoding behavior, including preamble handling and encoding selection.

```csharp
using System;
using System.Text;
using Cuemon.Text; // for EncodingOptions, PreambleSequence

namespace MyApp.Examples;

public class EncodingOptionsExample
{
    public void Demonstrate()
    {
        // Create options with UTF-8 encoding, removing the BOM preamble
        var options = new EncodingOptions
        {
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Remove
        };
        Console.WriteLine(options.Encoding.EncodingName); // Unicode (UTF-8)
        Console.WriteLine(options.Preamble);              // Remove

        // Create options that keep the preamble
        var preserveOptions = new EncodingOptions
        {
            Encoding = Encoding.Unicode,
            Preamble = PreambleSequence.Keep
        };
        Console.WriteLine(preserveOptions.Preamble); // Keep

        // Encoding property validates for null
        // options.Encoding = null; // throws ArgumentNullException

}
}

```
