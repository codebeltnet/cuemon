---
uid: Cuemon.Collections.Generic.DictionaryDecoratorExtensions
example:
- *content
---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Collections.Generic;

namespace Contoso.Inventory;

public sealed class DictionaryDecoratorExtensionsExample
{
    public static void Run()
    {
        IDictionary<int, string> catalog = new Dictionary<int, string>
        {
            [1] = "apples",
            [2] = "bananas"
        };

        var decorated = Decorator.Enclose(catalog);
        string fallback = decorated.GetValueOrDefault(3, () => "not found");
        bool found = decorated.TryGetValueOrFallback(42, keys => 2, out var alias);
        bool added = decorated.TryAdd(3, "cherries");
        decorated.AddOrUpdate(2, "blueberries");
        KeyValuePair<int, string>[] entries = decorated.ToEnumerable().ToArray();

        IDictionary<int, string> copy = decorated.CopyTo(new Dictionary<int, string>());

        IDictionary<int, Dictionary<int, int>> depthIndexes = new Dictionary<int, Dictionary<int, int>>();
        int depthIndex = Decorator.Enclose(depthIndexes).GetDepthIndex(readerDepth: 0, index: 1, nesting: 0);

        Console.WriteLine($"Fallback: {fallback}");
        Console.WriteLine($"Found fallback key: {found} -> {alias}");
        Console.WriteLine($"Added key 3: {added}");
        Console.WriteLine($"Enumerated entries: {entries.Length}");
        Console.WriteLine($"Copied entries: {copy.Count}");
        Console.WriteLine($"Depth index: {depthIndex}");
    }
}
```
