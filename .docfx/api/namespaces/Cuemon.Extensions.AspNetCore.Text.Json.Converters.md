---
uid: Cuemon.Extensions.AspNetCore.Text.Json.Converters
summary: *content
---
Register ASP.NET Core-specific `System.Text.Json` converters for HTTP types like `HttpExceptionDescriptor`, `StringValues`, `ProblemDetails`, and `HeaderDictionary`. Use this namespace when you need JSON serialization support for ASP.NET Core HTTP types. Start with `AddHttpExceptionDescriptorConverter` for structured error JSON or `AddStringValuesConverter` for header value serialization.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.Extensions.Text.Json.Converters namespace](/api/extensions/jsonnet/Cuemon.Extensions.Text.Json.Converters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|JsonConverter|⬇️|`AddHttpExceptionDescriptorConverter`, `AddStringValuesConverter`, `AddProblemDetailsConverter` and `AddHeaderDictionaryConverter`|
