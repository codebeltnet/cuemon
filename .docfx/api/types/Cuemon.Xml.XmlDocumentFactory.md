---
uid: Cuemon.Xml.XmlDocumentFactory
example:
- *content
---

The following example parses an XML string into an `XmlDocument` using `XmlDocumentFactory`, then navigates the document to read the root element name and a book's title attribute. The output displays "books" and "1984".

```csharp
// Program.cs
using System;
using System.Xml;
using Cuemon.Xml;

var xml = "<?xml version=\"1.0\"?><books><book title=\"1984\" author=\"Orwell\"/></books>";
var doc = XmlDocumentFactory.CreateDocument(xml);

var root = doc.DocumentElement;
Console.WriteLine($"Root element: {root?.Name}");

var book = root?.SelectSingleNode("book");
Console.WriteLine($"Title: {book?.Attributes?["title"]?.Value}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="92e896778d423e473597a609"><a href="~/run-actions/92e896778d423e473597a609/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/92e896778d423e473597a609/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
