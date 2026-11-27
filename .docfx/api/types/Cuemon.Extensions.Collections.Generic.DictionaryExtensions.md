---
uid: Cuemon.Extensions.Collections.Generic.DictionaryExtensions
example:
- *content
---

`DictionaryExtensions` provides extension methods for `IDictionary<TKey, TValue>` including `CopyTo`, `TryAdd` with a predicate, `AddOrUpdate`, `GetValueOrDefault` with a factory fallback, and `TryGetValueOrFallback`. This example creates a source dictionary with `"alpha": 1` and `"beta": 2`, copies entries to a new dictionary via `CopyTo`, conditionally adds `"gamma": 3` only if the key does not exist, updates `"beta"` to `20`, retrieves `"delta"` with a fallback factory returning `42`, and resolves a missing key using `TryGetValueOrFallback`. Console output confirms the destination count, fallback values, and the formatted key-value pairs.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Collections.Generic;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ac626c141f3b289e29fa5f19"><a href="~/run-actions/ac626c141f3b289e29fa5f19/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ac626c141f3b289e29fa5f19/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
