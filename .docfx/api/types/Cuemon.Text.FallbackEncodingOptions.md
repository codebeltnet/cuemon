---
uid: Cuemon.Text.FallbackEncodingOptions
example:
- *content
---

The following example demonstrates how to configure `FallbackEncodingOptions` to use exception fallbacks when encoding or decoding characters that are not supported by the target encoding.

```csharp
using System.Text;
using Cuemon.Text;

namespace MyApp.Examples;

public class FallbackEncodingOptionsExample
{
    public void Demonstrate()
    {
        var options = new FallbackEncodingOptions
        {
            TargetEncoding = Encoding.ASCII,
            EncoderFallback = EncoderFallback.ExceptionFallback,
            DecoderFallback = DecoderFallback.ExceptionFallback,
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Remove
        };

        // The options are ready to be used with encoding operations
        // that respect FallbackEncodingOptions. When an unsupported
        // character is encountered, an EncoderFallbackException will be thrown.

}
}

```
