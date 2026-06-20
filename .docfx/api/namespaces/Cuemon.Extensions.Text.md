---
uid: Cuemon.Extensions.Text
summary: *content
---
Detect character encodings and convert strings to their encoded byte representations without manual encoding logic. Use this namespace when you need encoding detection or string-to-encoded-byte conversion. Start with `DetectUnicodeEncoding` on `IEncodingOptions` for encoding detection, or `ToEncodedString` on `String` for encoding-aware string conversion.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Text namespace](/api/dotnet/Cuemon.Text.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IEncodingOptions|⬇️|`DetectUnicodeEncoding`|
|String|⬇️|`ToEncodedString`, `ToAsciiEncodedString`|
