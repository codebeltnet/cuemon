---
uid: Cuemon.Net.Collections.Specialized
summary: *content
---
Convert between `IDictionary<string, string[]>` and `NameValueCollection` using extension methods on the `IDecorator<IDictionary<string, string[]>>` type. The `Cuemon.Net.Collections.Specialized` namespace bridges generic dictionary parameters and legacy name-value collections for HTTP and form data scenarios. Use the `ToNameValueCollection` extension when you need to pass dictionary data to APIs that expect `NameValueCollection`. Start with this method on `IDecorator<IDictionary<string, string[]>>` for the most common conversion scenario.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Collections.Specialized namespace](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<IDictionary<string,string[]>>|⬇️|`ToNameValueCollection`|
|IDecorator<NameValueCollection>|⬇️|`ToString`|
