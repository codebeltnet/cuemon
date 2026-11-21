---
uid: Cuemon.Text.GuidStringOptions
example:
- *content
---

The following example demonstrates how to configure `GuidStringOptions` to restrict which GUID formats are accepted when parsing with `ParserFactory.FromGuid`.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Text;

// Direct instantiation of GuidStringOptions
var options = new GuidStringOptions
{
    Formats = GuidFormats.D
};

var parser = ParserFactory.FromGuid();
var guidString = "{3f2504e0-4f89-41d3-9a0c-0305e82c3301}";

var result = parser.Parse(guidString, o =>
{
    o.Formats = GuidFormats.B;
});

Console.WriteLine(result);
// outputs: 3f2504e0-4f89-41d3-9a0c-0305e82c3301
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d3ac33bea615aa19576fdd5d"><a href="~/run-actions/d3ac33bea615aa19576fdd5d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d3ac33bea615aa19576fdd5d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
