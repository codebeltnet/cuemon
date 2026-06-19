---
uid: Cuemon.Extensions.Collections.Specialized
summary: *content
---
Add dictionary-style operations like `ContainsKey` and `ToDictionary` to `NameValueCollection` for easier interoperability with generic collection types. Use this namespace when you need to bridge specialized `NameValueCollection` APIs with generic dictionary operations. Start with `ToDictionary` on `NameValueCollection` for LINQ integration, or `ToNameValueCollection` on `IDictionary{string, string[]}` for the reverse conversion.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Collections.Specialized namespace](/api/dotnet/Cuemon.Collections.Specialized.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDictionary{string, string[]}|⬇️|`ToNameValueCollection`|
|NameValueCollection|⬇️|`ContainsKey`, `ToDictionary`|
