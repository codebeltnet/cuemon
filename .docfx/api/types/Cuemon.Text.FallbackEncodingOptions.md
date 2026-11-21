---
uid: Cuemon.Text.FallbackEncodingOptions
example:
- *content
---

The following example demonstrates how to configure `FallbackEncodingOptions` to use exception fallbacks when encoding or decoding characters that are not supported by the target encoding.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Text;

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
Console.WriteLine(options.TargetEncoding.WebName);
Console.WriteLine(options.Preamble);
Console.WriteLine(options.EncoderFallback.MaxCharCount);
Console.WriteLine(options.DecoderFallback.MaxCharCount);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="78e348c26cd3eefc562ee250"><a href="~/run-actions/78e348c26cd3eefc562ee250/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/78e348c26cd3eefc562ee250/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
