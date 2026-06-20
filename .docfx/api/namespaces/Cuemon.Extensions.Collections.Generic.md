---
uid: Cuemon.Extensions.Collections.Generic
summary: *content
---
Partition, paginate, shuffle, and manage generic collections without writing custom extension methods. Use this namespace when you need safe, declarative collection operations like chunking, shuffling, ordering, or dictionary merging. Start with `Chunk<T>` or `ToPagination<T>` for batch processing of sequences, or `AddOrUpdate` on dictionaries for merge semantics.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Collections.Generic namespace](/api/dotnet/Cuemon.Collections.Generic.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection<T>|⬇️|`AddRange<T>`, `ToPartitioner<T>`|
|IEnumerable<T>|⬇️|`Chunk<T>`, `Shuffle<T>`, `OrderAscending<T>`, `OrderDescending<T>`, `RandomOrDefault<T>`, `ToPagination<T>`, `ToPaginationList<T>`, `ToPartitioner<T>`|
|IDictionary<TKey, TValue>|⬇️|`CopyTo<TKey, TValue>`, `GetValueOrDefault<TKey, TValue>`, `TryGetValueOrFallback<TKey, TValue>`, `ToEnumerable<TKey, TValue>`, `TryAdd<TKey, TValue>`, `AddOrUpdate<TKey, TValue>`|
|IEnumerable<T>|⬇️|`Chunk<T>`, `Shuffle<T>`, `OrderAscending<T>`, `OrderDescending<T>`, `RandomOrDefault<T>`, `ToPagination<T>`, `ToPaginationList<T>`|
|IEnumerable<KeyValuePair<TKey, TValue>>|⬇️|`ToDictionary<TKey, TValue>`|
|IList<T>|⬇️|`Remove<T>`, `HasIndex<T>`, `Next<T>`, `Previous<T>`, `TryAdd<T>`|
|Queue<T>|⬇️|`TryPeek<T>`|
|Stack<T>|⬇️|`TryPop<T>`|
|T|⬇️|`Yield<T>`|
