---
uid: Cuemon.Collections.Specialized
summary: *content
---
Convert between `IDictionary<string, string[]>` and `NameValueCollection` without writing manual key-value transformation code. Use this namespace when you need to bridge generic dictionaries and legacy name-value collections. Start with the `ToNameValueCollection` extension on `IDecorator<IDictionary<string, string[]>>`.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Collections.Specialized namespace](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<IDictionary<string,string[]>>|⬇️|`ToNameValueCollection`|
