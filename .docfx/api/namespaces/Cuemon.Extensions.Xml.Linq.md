---
uid: Cuemon.Extensions.Xml.Linq
summary: *content
---
Validate XML content with `IsXmlString` and safely parse XML strings into `XElement` instances without exceptions using `TryParseXElement`. Use this namespace when you need safe XML parsing that avoids throwing exceptions on malformed input. Start with `TryParseXElement` on `String` for exception-free XML parsing, or `IsXmlString` to validate XML content before processing.

Complements: [Cuemon.Xml.Linq namespace](/api/dotnet/Cuemon.Xml.Linq.html) 📘

[!INCLUDE [availability-default](../../includes/availability-default.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|String|⬇️|`IsXmlString`, `TryParseXElement`|
