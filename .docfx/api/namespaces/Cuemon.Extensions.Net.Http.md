---
uid: Cuemon.Extensions.Net.Http
summary: *content
---
Execute GET, POST, PUT, DELETE, PATCH, and other HTTP requests in a single line of code from a `Uri` without manually creating and configuring `HttpClient`. Use this namespace when you need concise HTTP calls from URI values. Start with `HttpGetAsync` on `Uri` for simple GET requests, or the generic `HttpAsync` method for any HTTP method.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|HttpMethod|⬇️|`ToHttpMethod`|
|Uri|⬇️|`HttpDeleteAsync`, `HttpGetAsync`, `HttpHeadAsync`, `HttpOptionsAsync`, `HttpPostAsync`, `HttpPutAsync`, `HttpPatchAsync`, `HttpTraceAsync`, `HttpAsync`|
