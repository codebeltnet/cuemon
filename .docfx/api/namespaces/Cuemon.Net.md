---
uid: Cuemon.Net
summary: *content
---
Work with HTTP and SMTP protocols through a simple programming interface. Use this namespace when you need to work with URI encoding, query strings, or protocol-level conversions. Start with `UrlEncode` on `IDecorator<String>` for encoding URI components, or `ToQueryString` for building query strings from name-value collections.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Net namespace](https://docs.microsoft.com/en-us/dotnet/api/system.net)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<Byte[]>|⬇️|`ToStream`, `UrlEncode`|
|IDecorator<String>|⬇️|`ToStream`, `UrlEncode`, `UrlDecode`, `ToQueryString`|
