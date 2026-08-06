using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;
using Cuemon.Runtime;
using Cuemon.Runtime.Caching;

namespace Cuemon.Extensions.Runtime.Caching;
/// <summary>
/// Extension methods for the <see cref="ICacheEnumerable{TKey}"/> interface.
/// </summary>
public static class CacheEnumerableExtensions
{
    /// <summary>
    /// Represents a cache with a scope of Memoization.
    /// </summary>
    public const string MemoizationScope = "Memoization";

    private const long MemoizationNullHashCode = 854726591;

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, IDependency dependency, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, CacheEntry.NoScope, dependency, valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache. Default is <see cref="CacheEntry.NoScope"/>.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, string ns, IDependency dependency, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, ns, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, IEnumerable<IDependency> dependencies, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, CacheEntry.NoScope, dependencies, valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache. Default is <see cref="CacheEntry.NoScope"/>.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, string ns, IEnumerable<IDependency> dependencies, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, ns, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, TimeSpan slidingExpiration, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, CacheEntry.NoScope, slidingExpiration, valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache. Default is <see cref="CacheEntry.NoScope"/>.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, string ns, TimeSpan slidingExpiration, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, ns, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, DateTime absoluteExpiration, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, CacheEntry.NoScope, absoluteExpiration, valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache. Default is <see cref="CacheEntry.NoScope"/>.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, string ns, DateTime absoluteExpiration, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, ns, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, CacheInvalidation invalidation, Func<TResult> valueFactory)
    {
        return GetOrAdd(cache, key, CacheEntry.NoScope, invalidation, valueFactory);
    }

    /// <summary>
    /// Gets the value for the specified <paramref name="key"/> from the cache, or if the <paramref name="key"/> does not exists, adds a value to the cache using the specified function delegate <paramref name="valueFactory"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache. Default is <see cref="CacheEntry.NoScope"/>.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a value for the <paramref name="key"/>.</param>
    /// <returns>The value for the specified <paramref name="key"/>. This will either be the existing value if the <paramref name="key"/> is already in the cache, or the new value returned by <paramref name="valueFactory"/> if the <paramref name="key"/> was not in the cache.</returns>
    public static TResult GetOrAdd<TKey, TResult>(this ICacheEnumerable<TKey> cache, string key, string ns, CacheInvalidation invalidation, Func<TResult> valueFactory)
    {
        Validator.ThrowIfNull(cache);
        Validator.ThrowIfNull(key);
        Validator.ThrowIfNull(invalidation);
        Validator.ThrowIfNull(valueFactory);

        if (!cache.TryGet(key, ns, out var value))
        {
            value = valueFactory();
            cache.Add(new CacheEntry(key, value, ns), invalidation);
        }
        return (TResult)value;
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<TResult> Memoize<TKey, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<TResult> Memoize<TKey, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<TResult> Memoize<TKey, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<TResult> Memoize<TKey, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<TResult> Memoize<TKey, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<TResult> valueFactory)
    {
        return delegate
        {
            var key = ComputeMemoizationCacheKey(valueFactory);
            var factory = new FuncFactory<MutableTuple, TResult>(_ => valueFactory(), new MutableTuple(), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T, TResult> Memoize<TKey, T, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<T, TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T, TResult> Memoize<TKey, T, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<T, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T, TResult> Memoize<TKey, T, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<T, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T, TResult> Memoize<TKey, T, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<T, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T, TResult> Memoize<TKey, T, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<T, TResult> valueFactory)
    {
        return delegate (T arg)
        {
            var key = ComputeMemoizationCacheKey(valueFactory, arg);
            var factory = new FuncFactory<MutableTuple<T>, TResult>(tuple => valueFactory(tuple.Arg1), new MutableTuple<T>(arg), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, TResult> Memoize<TKey, T1, T2, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<T1, T2, TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, TResult> Memoize<TKey, T1, T2, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<T1, T2, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, TResult> Memoize<TKey, T1, T2, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<T1, T2, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, TResult> Memoize<TKey, T1, T2, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<T1, T2, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, TResult> Memoize<TKey, T1, T2, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<T1, T2, TResult> valueFactory)
    {
        return delegate (T1 arg1, T2 arg2)
        {
            var key = ComputeMemoizationCacheKey(valueFactory, arg1, arg2);
            var factory = new FuncFactory<MutableTuple<T1, T2>, TResult>(tuple => valueFactory(tuple.Arg1, tuple.Arg2), new MutableTuple<T1, T2>(arg1, arg2), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, TResult> Memoize<TKey, T1, T2, T3, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<T1, T2, T3, TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, TResult> Memoize<TKey, T1, T2, T3, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<T1, T2, T3, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, TResult> Memoize<TKey, T1, T2, T3, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<T1, T2, T3, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, TResult> Memoize<TKey, T1, T2, T3, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<T1, T2, T3, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, TResult> Memoize<TKey, T1, T2, T3, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<T1, T2, T3, TResult> valueFactory)
    {
        return delegate (T1 arg1, T2 arg2, T3 arg3)
        {
            var key = ComputeMemoizationCacheKey(valueFactory, arg1, arg2, arg3);
            var factory = new FuncFactory<MutableTuple<T1, T2, T3>, TResult>(tuple => valueFactory(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, TResult> Memoize<TKey, T1, T2, T3, T4, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<T1, T2, T3, T4, TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, TResult> Memoize<TKey, T1, T2, T3, T4, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<T1, T2, T3, T4, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, TResult> Memoize<TKey, T1, T2, T3, T4, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, TResult> Memoize<TKey, T1, T2, T3, T4, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<T1, T2, T3, T4, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, TResult> Memoize<TKey, T1, T2, T3, T4, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<T1, T2, T3, T4, TResult> valueFactory)
    {
        return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var key = ComputeMemoizationCacheKey(valueFactory, arg1, arg2, arg3, arg4);
            var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4>, TResult>(tuple => valueFactory(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependency">An <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<TKey, T1, T2, T3, T4, T5, TResult>(this ICacheEnumerable<TKey> cache, IDependency dependency, Func<T1, T2, T3, T4, T5, TResult> valueFactory)
    {
        return Memoize(cache, Arguments.Yield(dependency), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<TKey, T1, T2, T3, T4, T5, TResult>(this ICacheEnumerable<TKey> cache, IEnumerable<IDependency> dependencies, Func<T1, T2, T3, T4, T5, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(dependencies), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<TKey, T1, T2, T3, T4, T5, TResult>(this ICacheEnumerable<TKey> cache, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(slidingExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached entry becomes invalid and is removed from the cache.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<TKey, T1, T2, T3, T4, T5, TResult>(this ICacheEnumerable<TKey> cache, DateTime absoluteExpiration, Func<T1, T2, T3, T4, T5, TResult> valueFactory)
    {
        return Memoize(cache, new CacheInvalidation(absoluteExpiration), valueFactory);
    }

    /// <summary>
    /// Memoizes the specified <paramref name="valueFactory"/> in the cache for fast access.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the cache.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="valueFactory" />.</typeparam>
    /// <typeparam name="TResult">The type of the value in the cache.</typeparam>
    /// <param name="cache">The <see cref="ICacheEnumerable{TKey}"/> to extend.</param>
    /// <param name="invalidation">The object that contains expiration details for a specific cache entry.</param>
    /// <param name="valueFactory">The function delegate used to provide a memoized value.</param>
    /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="valueFactory"/>.</returns>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<TKey, T1, T2, T3, T4, T5, TResult>(this ICacheEnumerable<TKey> cache, CacheInvalidation invalidation, Func<T1, T2, T3, T4, T5, TResult> valueFactory)
    {
        return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var key = ComputeMemoizationCacheKey(valueFactory, arg1, arg2, arg3, arg4, arg5);
            var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult>(tuple => valueFactory(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), valueFactory);
            return Memoize(cache, key, invalidation, factory);
        };
    }

#if NET9_0_OR_GREATER
    private readonly static System.Threading.Lock PadLock = new();
#else
    private readonly static object PadLock = new();
#endif

    private static TResult Memoize<TKey, TTuple, TResult>(ICacheEnumerable<TKey> cache, string key, CacheInvalidation invalidation, FuncFactory<TTuple, TResult> valueFactory) where TTuple : MutableTuple
    {
        if (cache.TryGetCacheEntry(key, MemoizationScope, out var cacheEntry)) { return (TResult)cacheEntry.Value; }
        lock (PadLock)
        {
            if (!cache.TryGet(key, MemoizationScope, out var value))
            {
                value = valueFactory.ExecuteMethod();
                cache.Add(new CacheEntry(key, value, MemoizationScope), invalidation);
            }
            return (TResult)value;
        }
    }

    private static string ComputeMemoizationCacheKey(Delegate del, params object[] args)
    {
        var result = del == null || del.GetMethodInfo() == null
            ? MemoizationNullHashCode.GetHashCode()
            : MethodDescriptor.Create(del.GetMethodInfo()).ToString().GetHashCode();

        foreach (var arg in args)
        {
            var current = arg ?? MemoizationNullHashCode;
            result ^= current is not byte[] bytes
                ? current.GetHashCode()
                : Generate.HashCode32(bytes.Cast<IConvertible>());
        }

        return result.ToString(CultureInfo.InvariantCulture);
    }
}
