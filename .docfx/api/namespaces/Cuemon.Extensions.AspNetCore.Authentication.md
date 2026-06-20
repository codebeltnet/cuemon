---
uid: Cuemon.Extensions.AspNetCore.Authentication
summary: *content
---
Register Basic, Digest, and HMAC authentication middleware in your ASP.NET Core pipeline with a single extension method call. Use this namespace when you need to enable HTTP authentication schemes in your application. Start with `UseBasicAuthentication` on `IApplicationBuilder` for basic auth, `UseDigestAccessAuthentication` for digest auth, or `UseHmacAuthentication` for HMAC auth.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Authentication namespace](/api/aspnet/Cuemon.AspNetCore.Authentication.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseBasicAuthentication`, `UseDigestAccessAuthentication`, `UseHmacAuthentication`|
|AuthenticationBuilder|⬇️|`AddBasic`, `AddDigestAccess`, `AddHmac`|
|IServiceCollection|⬇️|`AddInMemoryDigestAuthenticationNonceTracker`, `AddAuthorizationResponseHandler`|
