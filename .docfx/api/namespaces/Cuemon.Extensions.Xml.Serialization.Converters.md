---
uid: Cuemon.Extensions.Xml.Serialization.Converters
summary: *content
---
Register XML serialization converters for enumerables, exceptions, URIs, date/time types, and strings on `IList<XmlConverter>`. Use this namespace when you need custom XML converter registration beyond the default converters. Start with `AddXmlConverter` for generic converter registration or `AddStringConverter` for string-specific XML conversion.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Xml.Serialization.Converters namespace](/api/dotnet/Cuemon.Xml.Serialization.Converters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IList<XmlConverter>|⬇️|`FirstOrDefaultReaderConverter`, `FirstOrDefaultWriterConverter`, `AddXmlConverter<T>`, `InsertXmlConverter<T>`, `AddEnumerableConverter`, `AddExceptionDescriptorConverter`, `AddUriConverter`, `AddDateTimeConverter`, `AddTimeSpanConverter`, `AddStringConverter`, `AddExceptionConverter`, `AddFailureConverter`|
