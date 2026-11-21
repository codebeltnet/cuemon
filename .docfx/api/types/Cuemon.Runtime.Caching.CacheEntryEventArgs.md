---
uid: Cuemon.Runtime.Caching.CacheEntryEventArgs
example:
- *content
---

The following example demonstrates how <see cref="CacheEntryEventArgs" /> is delivered when a dependency invalidates a cache entry.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.Runtime;
using Cuemon.Runtime.Caching;
using MyApp.Examples;

CacheEntryEventArgsExample.Demonstrate();

namespace MyApp.Examples
{
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
            };

            cache.Add(entry, new CacheInvalidation(new[] { dependency }));
            dependency.SignalChanged();

            Console.WriteLine($"Cache entry expired: {captured is not null}");
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cf25fad2c9f63850f5f01e5a"><a href="~/run-actions/cf25fad2c9f63850f5f01e5a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cf25fad2c9f63850f5f01e5a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
