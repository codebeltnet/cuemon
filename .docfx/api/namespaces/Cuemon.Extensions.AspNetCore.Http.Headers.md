---
uid: Cuemon.Extensions.AspNetCore.Http.Headers
summary: *content
---
Register correlation identifiers, request identifiers, user-agent sentinel, API-key sentinel, and cache-control middleware on your ASP.NET Core pipeline with single extension method calls. Use this namespace when you need to add request correlation, user-agent validation, API-key protection, or cache-control headers to your pipeline. Start with `UseCorrelationIdentifier` for request tracing or `UseCacheControl` for HTTP cache headers.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Http.Headers namespace](/api/aspnet/Cuemon.AspNetCore.Http.Headers.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseCorrelationIdentifier`, `UseRequestIdentifier`, `UseUserAgentSentinel`, `UseApiKeySentinel`, `UseCacheControl`, `UseVaryAccept`|
|IServiceCollection|⬇️|`AddApiKeySentinelOptions`, `AddUserAgentSentinelOptions`|
