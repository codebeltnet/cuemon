---
uid: Cuemon.AspNetCore.Http.Headers
summary: *content
---
Add HTTP caching headers, correlation identifiers, API key protection, and conditional request handling using middleware components for ASP.NET Core. Use this namespace when you need cache validation, request correlation, API key sentinel protection, or ETag/last-modified header support. Start with `CacheableMiddleware` and `CacheableOptions` for HTTP caching scenarios, or `CorrelationIdentifierMiddleware` for correlating requests across services. For API key protection, use `ApiKeySentinelMiddleware`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Microsoft.AspNetCore.Http.Headers namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.headers) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<ChecksumBuilder>|⬇️|`ToEntityTagHeaderValue`|