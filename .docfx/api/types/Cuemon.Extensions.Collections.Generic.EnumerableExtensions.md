---
uid: Cuemon.Extensions.Collections.Generic.EnumerableExtensions
example:
- *content
---

`EnumerableExtensions` provides extension methods for `IEnumerable<T>` including chunking, shuffling, ordering, pagination, and materialization. This example starts with an `int[] { 4, 1, 3, 2 }` and applies `Chunk(2)` to split into batches, `Shuffle` with both random and deterministic seeds, `OrderAscending` and `OrderDescending` for sorted output, `RandomOrDefault` for element selection, and `Yield` to wrap a single value. It also demonstrates `ToDictionary` from `KeyValuePair` sequences, `ToPartitioner` for partitioned iteration, and `ToPagination`/`ToPaginationList` for page-based access. Console output confirms each operation's results, such as chunk count (`2`), sorted order (`1, 2, 3, 4`), and dictionary value for key `"beta"`.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Collections.Generic;
using CuemonEnumerableExtensions = Cuemon.Extensions.Collections.Generic.EnumerableExtensions;

namespace MyApp.Examples
{
    public static class EnumerableExtensionsExample
    {
        public static void Demonstrate()
        {
            var values = new[] { 4, 1, 3, 2 };
            var chunked = CuemonEnumerableExtensions.Chunk(values, 2).ToList();
            var shuffled = CuemonEnumerableExtensions.Shuffle(values).ToArray();
            var deterministicShuffle = CuemonEnumerableExtensions.Shuffle(values, (min, max) => min).ToArray();
            var ascending = values.OrderAscending().ToArray();
            var ascendingWithComparer = values.OrderAscending(Comparer<int>.Default).ToArray();
            var descending = CuemonEnumerableExtensions.OrderDescending(values).ToArray();
            var random = values.RandomOrDefault();
            var yielded = 5.Yield().Single();
            var dictionary = CuemonEnumerableExtensions.ToDictionary(new[]
            {
                new KeyValuePair<string, int>("alpha", 1),
                new KeyValuePair<string, int>("beta", 2)
            });
            var partitioner = values.ToPartitioner(2).ToList();
            var pagination = values.ToPagination(() => values.Length).ToList();
            var paginationList = values.ToPaginationList(() => values.Length);

            Console.WriteLine(chunked.Count);
            Console.WriteLine(shuffled.Length + deterministicShuffle.Length);
            Console.WriteLine(string.Join(", ", ascending));
            Console.WriteLine(string.Join(", ", ascendingWithComparer));
            Console.WriteLine(string.Join(", ", descending));
            Console.WriteLine(random);
            Console.WriteLine(yielded);
            Console.WriteLine(dictionary["beta"]);
            Console.WriteLine(partitioner.Count);
            Console.WriteLine(pagination.Count);
            Console.WriteLine(paginationList.Count);
        }
    }
}
```
