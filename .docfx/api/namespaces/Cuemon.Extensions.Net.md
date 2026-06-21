---
uid: Cuemon.Extensions.Net
summary: *content
---
Build query strings, encode and decode URLs, evaluate HTTP status codes concisely, and manage `HttpClient` instances without the boilerplate. Use this namespace when you need URL encoding, query string construction, HTTP status classification, or a lightweight `IHttpClientFactory`. Start with `ToQueryString` on `IDictionary{string, string[]}` for building query strings or `SlimHttpClientFactory` for managed HTTP clients.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Net namespace](/api/dotnet/Cuemon.Net.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`UrlEncode`|
|IDictionary{string, string[]}|⬇️|`ToQueryString`|
|HttpStatusCode|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|
|NameValueCollection|⬇️|`ToQueryString`|
|String|⬇️|`UrlDecode`, `UrlEncode`|
