---
uid: Cuemon.Extensions.Runtime.Caching
summary: *content
---
Cache expensive function results and reduce redundant computation through memoization and `GetOrAdd` patterns. Use this namespace when you need declarative caching with expiration management. Start with `GetOrAdd` on `ICacheEnumerable<TKey>` for simple caching, or `Memoize` for function result caching.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Runtime.Caching namespace](/api/dotnet/Cuemon.Runtime.Caching.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICacheEnumerable<TKey>|⬇️|`GetOrAdd<TKey, TResult>`, `Memoize<TKey, TResult>`, `Memoize<TKey, T, TResult>`, `Memoize<TKey, T1, T2, TResult>`, `Memoize<TKey, T1, T2, T3, TResult>`, `Memoize<TKey, T1, T2, T3, T4, TResult>`, `Memoize<TKey, T1, T2, T3, T4, T5, TResult>`|
