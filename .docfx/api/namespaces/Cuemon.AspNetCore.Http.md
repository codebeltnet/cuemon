---
uid: Cuemon.AspNetCore.Http
summary: *content
---
Handle structured HTTP errors, throttling, sentinel validation, and header manipulation in ASP.NET Core applications. Use this namespace when you need status-code exceptions, request throttling, API key validation, or HTTP header utilities. Start with status-code exception classes such as `BadRequestException`, `ForbiddenException`, or `NotFoundException` for standard HTTP error responses, or sentinel middleware like `ApiKeySentinelMiddleware` for request validation.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Microsoft.AspNetCore.Http namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http) 🔗

Related: [Cuemon.Extensions.AspNetCore.Http namespace](/api/extensions/aspnet/Cuemon.Extensions.AspNetCore.Http.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<HttpContext>|⬇️|`InvokeThrottlerSentinelAsync`, `InvokeUserAgentSentinelAsync`, `InvokeApiKeySentinelAsync`, `WriteExceptionDescriptorResponseAsync`|
|IDecorator<HttpRequest>|⬇️|`IsGetOrHeadMethod`, `IsClientSideResourceCached`|
|IDecorator<HttpResponse>|⬇️|`AddOrUpdateEntityTagHeader`, `AddOrUpdateLastModifiedHeader`|
|IDecorator<HttpStatusCodeException>|⬇️|`AddResponseHeaders<T>`|
|IDecorator<T>|⬇️|`AddResponseHeaders<T>`|
|IDecorator<IHeaderDictionary>|⬇️|`AddRange`, `AddOrUpdateHeader`, `AddOrUpdateHeaders`|
|IDecorator<Int32>|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsNotModifiedStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|