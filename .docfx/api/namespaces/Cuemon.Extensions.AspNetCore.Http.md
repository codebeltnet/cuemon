---
uid: Cuemon.Extensions.AspNetCore.Http
summary: *content
---
Manipulate HTTP headers, query HTTP status codes, and write response bodies on ASP.NET Core types without repetitive boilerplate. Use this namespace when you need to check HTTP method semantics on `HttpRequest`, manage ETag and Last-Modified headers on `HttpResponse`, or classify HTTP status codes. Start with `IsGetOrHeadMethod` on `HttpRequest` for method checks, or `AddOrUpdateEntityTagHeader` on `HttpResponse` for cache header management.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Http namespace](https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Http.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IHeaderDictionary|⬇️|`AddOrUpdateHeaders`, `AddOrUpdateHeader`|
|HttpRequest|⬇️|`IsGetOrHeadMethod`, `IsClientSideResourceCached`, `AcceptMimeTypesOrderedByQuality`|
|IEnumerable<IHttpExceptionDescriptorResponseFormatter>|⬇️|`SelectExceptionDescriptorHandlers`|
|HttpResponse|⬇️|`AddOrUpdateEntityTagHeader`, `AddOrUpdateLastModifiedHeader`, `WriteBodyAsync`, `OnStartingInvokeTransformer`|
|Int32|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsNotModifiedStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|
