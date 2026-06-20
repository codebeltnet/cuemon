---
uid: Cuemon.Xml.Serialization.Converters
summary: *content
---
Convert objects to and from XML using converters that follow the familiar [JsonConverter](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonConverter.htm) pattern from Newtonsoft.Json. Use this namespace when you need custom XML converter registration for strings, time spans, URIs, or custom types. Start with `AddStringConverter` or `AddXmlConverter<T>` on `ICollection<XmlConverter>` to register XML converters.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Related: [Cuemon.Extensions.Xml.Serialization.Converters namespace](/api/dotnet/Cuemon.Xml.Serialization.Converters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<ICollection<XmlConverter>>|⬇️|`AddStringConverter`, `AddTimeSpanConverter`, `AddUriConverter`, `AddXmlConverter<T>`, `InsertXmlConverter<T>`, `FirstOrDefaultReaderConverter`, `FirstOrDefaultWriterConverter`|
|IDecorator<IList<XmlConverter>>|⬇️|`AddXmlConverter<T>`, `InsertXmlConverter<T>`, `AddEnumerableConverter`, `AddExceptionDescriptorConverter`, `AddDateTimeConverter`, `AddExceptionConverter`, `AddFailureConverter`|
