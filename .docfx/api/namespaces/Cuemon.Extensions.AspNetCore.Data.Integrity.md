---
uid: Cuemon.Extensions.AspNetCore.Data.Integrity
summary: *content
---
Bridge data-integrity checksums and cache validation into ASP.NET Core HTTP infrastructure by converting `CacheValidator` and `ChecksumBuilder` instances into HTTP ETag header values. Use this namespace when you need conditional request handling based on content integrity checks. Start with `ToEntityTagHeaderValue` on a `ChecksumBuilder` to produce ETag headers for HTTP cache validation.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.Data.Integrity namespace](/api/dotnet/Cuemon.Data.Integrity.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|CacheValidator|⬇️|`ToEntityTagHeaderValue`|
|ChecksumBuilder|⬇️|`ToEntityTagHeaderValue`|