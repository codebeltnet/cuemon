---
uid: Cuemon.Net
summary: *content
---
Work with HTTP and SMTP protocols through a simple programming interface. Use this namespace when you need to work with URI encoding, query strings, or protocol-level conversions. Start with `UrlEncode` and `UrlDecode` on `IDecorator<String>` for encoding and decoding URI components, or see [Cuemon.Extensions.Net](/api/extensions/dotnet/Cuemon.Extensions.Net.html) for `ToQueryString` on name-value collections.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Net namespace](https://docs.microsoft.com/en-us/dotnet/api/system.net)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<Byte[]>|⬇️|`ToStream`, `UrlEncode`|
|IDecorator<String>|⬇️|`ToStream`, `UrlEncode`, `UrlDecode`|
