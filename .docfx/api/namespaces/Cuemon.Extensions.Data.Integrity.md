---
uid: Cuemon.Extensions.Data.Integrity
summary: *content
---
Generate cache validators, combine checksums, and compute content integrity hashes for caching and validation scenarios. Use this namespace when you need data integrity checks based on assembly versions, file timestamps, or content hashes. Start with `GetCacheValidator` on `Assembly` or `FileInfo` to generate a cache validator for integrity checks.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Data.Integrity namespace](/api/dotnet/Cuemon.Data.Integrity.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|Assembly|⬇️|`GetCacheValidator`|
|ChecksumBuilder|⬇️|`CombineWith<T>`|
|T|⬇️|`CombineWith<T>`|
|DateTime|⬇️|`GetCacheValidator`|
|FileInfo|⬇️|`GetCacheValidator`|
