using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cuemon.Collections.Generic;
using Cuemon.Threading;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Represents the type that implements an in-memory cache for an application.
/// </summary>
/// <seealso cref="Disposable" />
/// <seealso cref="ICacheEnumerable{Int64}" />
public class SlimMemoryCache : Disposable, ICacheEnumerable<long>
{
    private readonly ConcurrentDictionary<long, CacheEntry> _innerCaches = new();
    private readonly Timer _expirationTimer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SlimMemoryCache"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="SlimMemoryCacheOptions"/> which may be configured.</param>
    public SlimMemoryCache(Action<SlimMemoryCacheOptions> setup = null)
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        KeyProvider = options.KeyProvider;
        if (options.EnableCleanup)
        {
            _expirationTimer = TimerFactory.CreateNonCapturingTimer(state => ((SlimMemoryCache)state!).OnAutomatedSweepCleanup(), this, options.FirstSweep, options.SucceedingSweep);
        }
    }

    /// <summary>
    /// Gets or sets a value in the cache by using the default indexer property for an instance of the <see cref="SlimMemoryCache"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the cache value to get or set.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>The value in the cache for the specified <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    public object this[string key, string ns = CacheEntry.NoScope]
    {
        get
        {
            if (TryGetCacheEntry(key, ns, out var cache))
            {
                return cache.Value;
            }
            return null;
        }
        set
        {
            if (TryGetCacheEntry(key, ns, out var cache))
            {
                cache.Value = value;
            }
        }
    }

    /// <summary>
    /// Gets the function delegate that is responsible for providing a unique identifier for the cache entry.
    /// </summary>
    /// <value>The function delegate that is responsible for providing a unique identifier for the cache entry.</value>
    public Func<string, string, long> KeyProvider { get; }

    /// <summary>
    /// Inserts a cache entry into the cache and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached <paramref name="value"/> becomes invalid and is removed from the cache.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the cache with the same key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool Add(string key, object value, DateTime absoluteExpiration, string ns = CacheEntry.NoScope)
    {
        return Add(new CacheEntry(key, value, ns), new CacheInvalidation(absoluteExpiration));
    }

    /// <summary>
    /// Inserts a cache entry into the cache and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached <paramref name="value"/> becomes invalid and is removed from the cache.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the cache with the same key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool Add(string key, object value, IDependency dependency, string ns = CacheEntry.NoScope)
    {
        return Add(new CacheEntry(key, value, ns), new CacheInvalidation(Arguments.Yield(dependency)));
    }

    /// <summary>
    /// Inserts a cache entry into the cache and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached <paramref name="value"/> becomes invalid and is removed from the cache.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the cache with the same key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool Add(string key, object value, IEnumerable<IDependency> dependencies, string ns = CacheEntry.NoScope)
    {
        return Add(new CacheEntry(key, value, ns), new CacheInvalidation(dependencies));
    }

    /// <summary>
    /// Inserts a cache entry into the cache and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached <paramref name="value"/> becomes invalid and is removed from the cache.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the cache with the same key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool Add(string key, object value, TimeSpan slidingExpiration, string ns = CacheEntry.NoScope)
    {
        return Add(new CacheEntry(key, value, ns), new CacheInvalidation(slidingExpiration));
    }

    /// <summary>
    /// Inserts a cache entry into the cache and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="entry">The object representing the cached value for a cache entry.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the cache with the same key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entry"/> cannot be null -or-
    /// <paramref name="invalidation"/> cannot be null.
    /// </exception>
    public bool Add(CacheEntry entry, CacheInvalidation invalidation)
    {
        Validator.ThrowIfNull(entry);
        Validator.ThrowIfNull(invalidation);
        var nsKey = KeyProvider(entry.Key, entry.Namespace);
        return _innerCaches.TryAdd(nsKey, entry.SetInvalidation(invalidation).StartDependencies());
    }

    /// <summary>
    /// Determines whether a cache entry exists in the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if the cache contains a cache entry whose key matches <paramref name="key"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool Contains(string key, string ns = CacheEntry.NoScope)
    {
        return TryGetCacheEntry(key, ns, out _);
    }

    /// <summary>
    /// Gets the number of entries associated with the <paramref name="ns"/> contained in the cache.
    /// </summary>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>The number of entries contained in the cache.</returns>
    public int Count(string ns = CacheEntry.NoScope)
    {
        return ListCacheEntries(ns).Count;
    }

    /// <summary>
    /// Removes all entries associated with the <paramref name="ns"/> contained in the cache.
    /// </summary>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    public void RemoveAll(string ns = CacheEntry.NoScope)
    {
        var entries = ListCacheEntries(ns);
        foreach (var cacheEntry in entries)
        {
            Remove(cacheEntry.Key, cacheEntry.Namespace);
        }
    }

    /// <summary>
    /// Returns an entry from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>A reference to the value in the cache container that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public object Get(string key, string ns = CacheEntry.NoScope)
    {
        return GetCacheEntry(key, ns)?.Value;
    }

    /// <summary>
    /// Returns an entry from the cache as a <see cref="CacheEntry"/> instance.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>A reference to the <see cref="CacheEntry"/> that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public CacheEntry GetCacheEntry(string key, string ns = CacheEntry.NoScope)
    {
        if (TryGetCacheEntry(key, ns, out var cacheEntry))
        {
            return cacheEntry;
        }
        return null;
    }

    /// <summary>
    /// Removes a cache entry from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>If the entry is found in the cache, a reference to the value in the cache container of the removed cache entry; otherwise, <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public object Remove(string key, string ns = CacheEntry.NoScope)
    {
        Validator.ThrowIfNull(key);
        var nsKey = KeyProvider(key, ns);
        if (_innerCaches.TryRemove(nsKey, out var cacheEntry))
        {
            return cacheEntry.Value;
        }
        return null;
    }

    /// <summary>
    /// Inserts a cache entry into the cache.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null -or-
    /// <paramref name="invalidation"/> cannot be null.
    /// </exception>
    /// <remarks>The <see cref="Set"/> method always puts a cache value in the cache, regardless whether an entry already exists with the same key. If the specified entry does not exist in the cache, a new cache entry is inserted. If the specified entry exists, its value is updated.</remarks>
    public void Set(string key, object value, CacheInvalidation invalidation, string ns = CacheEntry.NoScope)
    {
        Validator.ThrowIfNull(key);
        if (TryGetCacheEntry(key, ns, out var cacheEntry))
        {
            cacheEntry.Value = value;
            cacheEntry.Refresh();
        }
        else
        {
            Add(new CacheEntry(key, value, ns), invalidation);
        }
    }

    /// <summary>
    /// Attempts to get the <see cref="CacheEntry"/> associated with the specified <paramref name="key"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="cacheEntry">When this method returns, contains the cache entry associated with the specified <paramref name="key"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="cacheEntry"/> was found in the cache; otherwise, <c>false</c>.</returns>
    public bool TryGetCacheEntry(string key, out CacheEntry cacheEntry)
    {
        return TryGetCacheEntry(key, CacheEntry.NoScope, out cacheEntry);
    }

    /// <summary>
    /// Attempts to get the <see cref="CacheEntry"/> associated with the specified <paramref name="key"/> and <paramref name="ns"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <param name="cacheEntry">When this method returns, contains the cache entry associated with the specified <paramref name="key"/> and <paramref name="ns"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="cacheEntry"/> was found in the cache; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool TryGetCacheEntry(string key, string ns, out CacheEntry cacheEntry)
    {
        Validator.ThrowIfNull(key);
        cacheEntry = null;
        var utcNow = DateTime.UtcNow;
        var nsKey = KeyProvider(key, ns);
        if (_innerCaches.TryGetValue(nsKey, out var ce))
        {
            var hasCacheExpired = ce.HasExpired(utcNow);
            if (ce.CanExpire && hasCacheExpired)
            {
                Remove(key, ns);
                return false;
            }

            if (ce.CanExpire && !hasCacheExpired)
            {
                cacheEntry = ce;
                ce.Refresh();
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to get the value associated with the specified <paramref name="key"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was found in the cache; otherwise, <c>false</c>.</returns>
    public bool TryGet(string key, out object value)
    {
        return TryGet(key, CacheEntry.NoScope, out value);
    }

    /// <summary>
    /// Attempts to get the value associated with the specified <paramref name="key"/> and <paramref name="ns"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/> and <paramref name="ns"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was found in the cache; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public bool TryGet(string key, string ns, out object value)
    {
        var success = TryGetCacheEntry(key, ns, out var cacheEntry);
        value = success ? cacheEntry.Value : null;
        return success;
    }

    private IList<CacheEntry> ListCacheEntries(string ns)
    {
        var utcNow = DateTime.UtcNow;
        var entries = new List<CacheEntry>();
        var snapshot = new List<CacheEntry>(_innerCaches.Values);
        foreach (var cacheEntry in snapshot)
        {
            if (cacheEntry == null) { continue; } // this can happen if a cache has been removed
            if (cacheEntry.CanExpire && cacheEntry.HasExpired(utcNow)) { continue; }
            if (cacheEntry.Namespace == ns)
            {
                entries.Add(cacheEntry);
            }
        }
        return entries;
    }

    /// <summary>
    /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
    /// </summary>
    protected override void OnDisposeManagedResources()
    {
        _expirationTimer?.Dispose();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<KeyValuePair<long, CacheEntry>> GetEnumerator()
    {
        return _innerCaches.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"></see> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void OnAutomatedSweepCleanup()
    {
        var utcNow = DateTime.UtcNow;
        var snapshot = new List<CacheEntry>(_innerCaches.Values);
        if (snapshot.Count > 0)
        {
            foreach (var cacheEntry in snapshot)
            {
                if (cacheEntry == null) { continue; }
                if (cacheEntry.CanExpire && cacheEntry.HasExpired(utcNow)) { Remove(cacheEntry.Key, cacheEntry.Namespace); }
            }
        }
    }
}
