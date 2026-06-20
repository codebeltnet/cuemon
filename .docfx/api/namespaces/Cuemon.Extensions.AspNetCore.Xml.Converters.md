---
uid: Cuemon.Extensions.AspNetCore.Xml.Converters
summary: *content
---
Register ASP.NET Core-specific XML serialization converters for HTTP types like `StringValues`, `HeaderDictionary`, `QueryCollection`, `FormCollection`, `CookieCollection`, and `ProblemDetails`. Use this namespace when you need XML serialization support for ASP.NET Core HTTP types. Start with `AddHttpExceptionDescriptorConverter` for structured error XML or `AddStringValuesConverter` for header value serialization.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.Extensions.Xml.Serialization.Converters namespace](/api/extensions/dotnet/Cuemon.Extensions.Xml.Serialization.Converters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|XmlConverter|⬇️|`AddHttpExceptionDescriptorConverter`, `AddStringValuesConverter`, `AddHeaderDictionaryConverter`, `AddQueryCollectionConverter`, `AddFormCollectionConverter`, `AddCookieCollectionConverter` and `AddProblemDetailsConverter`|
