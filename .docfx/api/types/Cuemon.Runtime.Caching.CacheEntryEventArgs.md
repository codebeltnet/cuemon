---
uid: Cuemon.Runtime.Caching.CacheEntryEventArgs
example:
- *content
---

The following example demonstrates how <see cref="CacheEntryEventArgs" /> is delivered when a dependency invalidates a cache entry.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.Runtime;
using Cuemon.Runtime.Caching;

namespace MyApp.Examples;

public static class CacheEntryEventArgsExample
{
    public static void Demonstrate()
    {
        var cache = new SlimMemoryCache();
        var dependency = new DependencyStub();
        var entry = new CacheEntry("key", "value");
        CacheEntryEventArgs captured = null;

        entry.Expired += (_, e) =>
        {
            captured = e;
            Console.WriteLine(e.GetType().Name);
        };

        cache.Add(entry, new CacheInvalidation(new[] { dependency }));
        dependency.SignalChanged();

        Console.WriteLine(captured != null);
    }

    private sealed class DependencyStub : IDependency
    {
        public event EventHandler<DependencyEventArgs> DependencyChanged;

        public DateTime? UtcLastModified { get; private set; }

        public bool HasChanged { get; private set; }

        public void Start()
        {
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public void SignalChanged()
        {
            UtcLastModified = DateTime.UtcNow;
            HasChanged = true;
            DependencyChanged?.Invoke(this, new DependencyEventArgs(UtcLastModified.Value));
        }
    }
}
```
