using System;
using System.Collections.Generic;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// An interface that is used to provide cache implementations for an application.
/// </summary>
/// <typeparam name="TKey">The type of the key in the cache.</typeparam>
public interface ICacheEnumerable<TKey> : IEnumerable<KeyValuePair<TKey, CacheEntry>>
{
    /// <summary>
    /// Gets or sets a value in the cache by using the default indexer property for an instance of the <see cref="SlimMemoryCache"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the cache value to get or set.</param>
    /// <param name="ns">The optional named group associated with the cache value.</param>
    /// <returns>The value in the cache for the specified <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    object this[string key, string ns = CacheEntry.NoScope]
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the function delegate that is responsible for providing a unique identifier for the cache entry.
    /// </summary>
    /// <value>The function delegate that is responsible for providing a unique identifier for the cache entry.</value>
    Func<string, string, TKey> KeyProvider { get; }

    /// <summary>
    /// Inserts a cache entry into the cache as a <see cref="CacheEntry"/> instance, and adds details about how the entry should be evicted.
    /// </summary>
    /// <param name="entry">The object representing the cached value for a cache entry.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    bool Add(CacheEntry entry, CacheInvalidation invalidation);

    /// <summary>
    /// Determines whether a cache entry exists in the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns><c>true</c> if the cache contains a cache entry whose key matches <paramref name="key"/>; otherwise, <c>false</c>.</returns>
    bool Contains(string key, string ns = CacheEntry.NoScope);

    /// <summary>
    /// Gets the number of entries associated with the <paramref name="ns"/> contained in the cache.
    /// </summary>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>The number of entries contained in the cache.</returns>
    int Count(string ns = CacheEntry.NoScope);

    /// <summary>
    /// Removes all entries associated with the <paramref name="ns"/> contained in the cache.
    /// </summary>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    void RemoveAll(string ns = CacheEntry.NoScope);

    /// <summary>
    /// Returns an entry from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>A reference to the value in the cache container that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    object Get(string key, string ns = CacheEntry.NoScope);

    /// <summary>
    /// Returns an entry from the cache as a <see cref="CacheEntry"/> instance.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>A reference to the <see cref="CacheEntry"/> that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
    CacheEntry GetCacheEntry(string key, string ns = CacheEntry.NoScope);

    /// <summary>
    /// Removes a cache entry from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <returns>If the entry is found in the cache, a reference to the value in the cache container of the removed cache entry; otherwise, <c>null</c>.</returns>
    object Remove(string key, string ns = default);

    /// <summary>
    /// Inserts a cache entry into the cache.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    void Set(string key, object value, CacheInvalidation invalidation, string ns = CacheEntry.NoScope);

    /// <summary>
    /// Attempts to get the <see cref="CacheEntry"/> associated with the specified <paramref name="key"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="cacheEntry">When this method returns, contains the cache entry associated with the specified <paramref name="key"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="cacheEntry"/> was found in the cache; otherwise, <c>false</c>.</returns>
    bool TryGetCacheEntry(string key, out CacheEntry cacheEntry);

    /// <summary>
    /// Attempts to get the <see cref="CacheEntry"/> associated with the specified <paramref name="key"/> and <paramref name="ns"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <param name="cacheEntry">When this method returns, contains the cache entry associated with the specified <paramref name="key"/> and <paramref name="ns"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="cacheEntry"/> was found in the cache; otherwise, <c>false</c>.</returns>
    bool TryGetCacheEntry(string key, string ns, out CacheEntry cacheEntry);

    /// <summary>
    /// Attempts to get the value associated with the specified <paramref name="key"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was found in the cache; otherwise, <c>false</c>.</returns>
    bool TryGet(string key, out object value);

    /// <summary>
    /// Attempts to get the value associated with the specified <paramref name="key"/> and <paramref name="ns"/> from the cache.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/> and <paramref name="ns"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was found in the cache; otherwise, <c>false</c>.</returns>
    bool TryGet(string key, string ns, out object value);
}
