---
uid: Cuemon.Extensions.Xml
summary: *content
---
Parse streams and byte arrays into `XmlReader`, escape or sanitize XML text, traverse `XmlReader` hierarchies, write structured XML with custom element wrappers, and remove XML namespace declarations. Use this namespace when you need comprehensive XML processing without low-level XML API boilerplate. Start with `ToXmlReader` on `Stream` or `byte[]` for XML parsing, or `EscapeXml` on `String` for XML-safe text.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Xml namespace](/api/dotnet/Cuemon.Xml.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToXmlReader`|
|DateTime|⬇️|`ToString`|
|IHierarchy<T>|⬇️|`HasXmlIgnoreAttribute`, `IsNodeEnumerable`, `GetXmlQualifiedEntity`|
|IEnumerable<IHierarchy<T>>|⬇️|`OrderByXmlAttributes<T>`|
|XmlWriter|⬇️|`WriteObject<T>`, `WriteStartElement`, `WriteEncapsulatingElementWhenNotNull<T>`, `WriteXmlRootElement<T>`, `WriteObject`|
|Stream|⬇️|`ToXmlReader`, `CopyXmlStream`, `TryDetectXmlEncoding`, `RemoveXmlNamespaceDeclarations`|
|String|⬇️|`EscapeXml`, `UnescapeXml`, `SanitizeXmlElementName`, `SanitizeXmlElementText`|
|Uri|⬇️|`ToXmlReader`|
|XmlReader|⬇️|`Chunk`, `ToHierarchy`, `ToStream`, `MoveToFirstElement`|
|XmlWriter|⬇️|`WriteObject<T>`, `WriteStartElement`, `WriteEncapsulatingElementWhenNotNull<T>`, `WriteXmlRootElement<T>`|
