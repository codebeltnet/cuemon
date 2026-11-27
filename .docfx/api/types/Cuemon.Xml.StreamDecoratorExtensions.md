---
uid: Cuemon.Xml.StreamDecoratorExtensions
example:
- *content
---

The following example demonstrates how to create an <see cref="XmlReader" /> from a stream and detect the XML encoding.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon;
using Cuemon.Xml;

const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><child>hello</child></root>";

using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
using XmlReader reader = Decorator.Enclose(stream).ToXmlReader();
reader.MoveToContent();

Console.WriteLine(reader.LocalName);

using var detectStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
var detected = Decorator.Enclose(detectStream).TryDetectXmlEncoding(out var encoding);

Console.WriteLine(detected);
Console.WriteLine(encoding.EncodingName);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b4155603fad628f1354816f1"><a href="~/run-actions/b4155603fad628f1354816f1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b4155603fad628f1354816f1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
