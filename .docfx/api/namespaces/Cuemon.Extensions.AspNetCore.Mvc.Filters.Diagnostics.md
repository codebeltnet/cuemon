---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics
summary: *content
---
Register custom fault resolvers for mapping exceptions to HTTP responses in the ASP.NET Core MVC diagnostics pipeline. Use this namespace when you need to extend the error-handling pipeline with custom exception-to-response mappings. Start with `AddHttpFaultResolver<T>` on `HttpFaultResolver` to register a custom fault resolver.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace](/api/aspnet/Cuemon.AspNetCore.Mvc.Filters.Diagnostics.html) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|HttpFaultResolver|⬇️|`AddHttpFaultResolver<T>`|
|IList<HttpFaultResolver>|⬇️|`AddHttpFaultResolver<T>`|
