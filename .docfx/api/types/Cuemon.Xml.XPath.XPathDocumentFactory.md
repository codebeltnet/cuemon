---
uid: Cuemon.Xml.XPath.XPathDocumentFactory
example:
- *content
---

The following example creates an `XPathDocument` from an XML string using `XPathDocumentFactory`, then queries it with an XPath expression to retrieve the text content of an `<item>` element. The resulting value "Hello" is written to the console.

```csharp
// Program.cs
using System;
using System.Xml.XPath;
using Cuemon.Xml.XPath;

var xml = "<root><item id=\"1\">Hello</item></root>";
var doc = XPathDocumentFactory.CreateDocument(xml);

var navigator = doc.CreateNavigator();
var value = navigator.SelectSingleNode("//item/text()")?.Value;
Console.WriteLine($"XPath result: {value}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b6f1f250a405c0ea37510f35"><a href="~/run-actions/b6f1f250a405c0ea37510f35/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b6f1f250a405c0ea37510f35/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
