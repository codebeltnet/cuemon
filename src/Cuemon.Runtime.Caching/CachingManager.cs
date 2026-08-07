using System;
using System.Threading;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Provides access to caching in an application.
/// </summary>
public static class CachingManager
{
    private static readonly Lazy<SlimMemoryCache> Singleton = new(() => new SlimMemoryCache(), LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    /// Gets a singleton instance of <see cref="SlimMemoryCache"/> that is an in-memory cache for an application.
    /// </summary>
    /// <value>A singleton instance of <see cref="SlimMemoryCache"/> that is an in-memory cache for an application.</value>
    public static SlimMemoryCache Cache => Singleton.Value;
}
