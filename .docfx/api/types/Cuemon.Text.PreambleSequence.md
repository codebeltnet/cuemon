---
uid: Cuemon.Text.PreambleSequence
example:
- *content
---

The following example demonstrates how to use `PreambleSequence` with `EncodingOptions` to control whether byte order marks (BOM) are preserved or removed.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Text;

var options = new EncodingOptions
{
    Encoding = Encoding.UTF8,
    Preamble = PreambleSequence.Keep
};

var preambleAction = options.Preamble;
var encoding = options.Encoding;

Console.WriteLine(preambleAction); // outputs: Keep
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="65d6560bf8c29efc2c046ccc"><a href="~/run-actions/65d6560bf8c29efc2c046ccc/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/65d6560bf8c29efc2c046ccc/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
