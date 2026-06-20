---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json
summary: *content
---
Register JSON formatters for ASP.NET Core MVC based on `System.Text.Json` with a single extension method call. Use this namespace when you need `System.Text.Json` formatters in your MVC pipeline. Start with `AddJsonFormatters` on `IMvcBuilder` or `IMvcCoreBuilder` to enable JSON serialization in your controllers.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.Extensions.Text.Json namespace](/api/extensions/jsonnet/Cuemon.Extensions.Text.Json.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|HttpExceptionDescriptorResponseHandler|⬇️|`AddJsonResponseHandler`|
|IMvcBuilder|⬇️|`AddJsonFormatters`, `AddJsonFormattersOptions`|
|IMvcCoreBuilder|⬇️|`AddJsonFormatters`, `AddJsonFormattersOptions`|
