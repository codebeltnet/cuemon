---
uid: Cuemon.Text.PreambleSequence
example:
- *content
---

The following example demonstrates how to use `PreambleSequence` with `EncodingOptions` to control whether byte order marks (BOM) are preserved or removed.

```csharp
using System;
using System.Text;
using Cuemon.Text;

namespace MyApp.Examples;

public class PreambleSequenceExample
{
    public void Demonstrate()
    {
        var options = new EncodingOptions
        {
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Keep
        };

        var preambleAction = options.Preamble;
        var encoding = options.Encoding;

        Console.WriteLine(preambleAction); // outputs: Keep

}
}

```
