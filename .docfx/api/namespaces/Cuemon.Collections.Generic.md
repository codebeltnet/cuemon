---
uid: Cuemon.Collections.Generic
summary: *content
---
Paginate, partition, and sort data using generic collection types that extend the .NET Base Class Library — including paginated collections, partitioned enumerables, dynamic comparers, a read-only enum dictionary, and a conditional collection. Use these types when you need paging, partitioning, or custom comparison semantics in your data processing. Start with `PaginationEnumerable<T>` or `PaginationList<T>` for paginated results, `PartitionerCollection<T>` or `PartitionerEnumerable<T>` for partitioned processing, or `ConditionalCollection<T>` and `DynamicComparer<T>` for flexible filtering and sorting without custom comparer classes.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Collections.Generic namespace](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic) 🔗

Related: [Cuemon.Extensions.Collections.Generic namespace](/api/extensions/dotnet/Cuemon.Extensions.Collections.Generic.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<ICollection<T>>|⬇️|`AddRange<T>`|
|IDecorator<IDictionary<TKey,TValue>>|⬇️|`CopyTo<TKey,TValue>`, `GetValueOrDefault<TKey,TValue>`, `TryGetValueOrFallback<TKey,TValue>`, `ToEnumerable<TKey,TValue>`, `TryAdd<TKey,TValue>`, `AddOrUpdate<TKey,TValue>`, `GetDepthIndex`|
|IDecorator<Stack<T>>|⬇️|`TryPop<T>`|
