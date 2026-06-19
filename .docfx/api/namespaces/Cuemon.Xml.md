---
uid: Cuemon.Xml
summary: *content
---
Serialize, encode, convert, and transform XML data with a lightweight XML serializer framework offering flexibility comparable to the JSON equivalent from Newtonsoft. Use this namespace when you need XML encoding detection, reader chunking, structured XML writing, or serialization. Start with `ToXmlReader` on `IDecorator<Stream>` for XML parsing, or `EscapeXml` on `IDecorator<String>` for XML-safe text.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Xml namespace](https://docs.microsoft.com/en-us/dotnet/api/system.xml) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<IHierarchy<Object>>|⬇️|`HasXmlIgnoreAttribute`, `IsNodeEnumerable`, `GetXmlQualifiedEntity`, `TryGetXmlTextAttribute`, `TryGetXmlAttributeAttribute`, `TryGetXmlRootAttribute`, `TryGetXmlElementAttribute`|
|IDecorator<IEnumerable<IHierarchy<T>>>|⬇️|`OrderByXmlAttributes<T>`|
|IDecorator<Stream>|⬇️|`ToXmlReader`, `TryDetectXmlEncoding`|
|IDecorator<String>|⬇️|`EscapeXml`, `UnescapeXml`, `SanitizeXmlElementName`, `SanitizeXmlElementText`|
|IDecorator<XmlReader>|⬇️|`Chunk`, `MoveToFirstElement`, `ToHierarchy`|
|IDecorator<XmlWriter>|⬇️|`WriteStartElement`, `WriteObject`, `WriteObject<T>`, `WriteEncapsulatingElementIfNotNull<T>`, `WriteXmlRootElement`, `WriteXmlRootElement<T>`|

Related: [Cuemon.Extensions.Xml namespace](/api/extensions/dotnet/Cuemon.Extensions.Xml.html) 📘
