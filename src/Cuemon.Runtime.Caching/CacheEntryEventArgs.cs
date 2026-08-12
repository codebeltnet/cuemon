using System;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Provides data for cache related operations. This class cannot be inherited.
/// </summary>
public sealed class CacheEntryEventArgs : EventArgs
{
    internal CacheEntryEventArgs(CacheEntry cache)
    {
        Cache = cache;
    }

    internal CacheEntry Cache { get; }
}
