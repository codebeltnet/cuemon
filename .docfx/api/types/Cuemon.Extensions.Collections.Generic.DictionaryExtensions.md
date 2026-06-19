---
uid: Cuemon.Extensions.Collections.Generic.DictionaryExtensions
example:
- *content
---

The following example demonstrates how to populate, update, and query a dictionary through the available extension methods.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Collections.Generic;

namespace MyApp.Examples
{
    public static class DictionaryExtensionsExample
    {
        public static void Demonstrate()
        {
            IDictionary<string, int> source = new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2
            };

            var destination = new Dictionary<string, int>();
            source.CopyTo(destination);

            source.TryAdd("gamma", 3, dictionary => !dictionary.ContainsKey("gamma"));
            source.AddOrUpdate("beta", 20);

            var configuredFallback = source.GetValueOrDefault("delta", () => 42);
            var foundFallback = source.TryGetValueOrFallback("missing", keys => keys.OrderBy(key => key).First(), out var fallbackValue);
            var rows = source.ToEnumerable().Select(pair => $"{pair.Key}:{pair.Value}");

            Console.WriteLine(destination.Count);
            Console.WriteLine(configuredFallback);
            Console.WriteLine(foundFallback);
            Console.WriteLine(fallbackValue);
            Console.WriteLine(string.Join(", ", rows));
        }
    }
}
```
