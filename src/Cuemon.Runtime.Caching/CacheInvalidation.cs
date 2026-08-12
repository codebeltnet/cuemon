using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Represents a set of eviction and expiration details for a specific cache entry.
/// </summary>
public class CacheInvalidation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheInvalidation"/> class.
    /// </summary>
    /// <param name="absoluteExpiration">The absolute expiration date time value from when the cached value becomes invalid and is removed from the cache.</param>
    public CacheInvalidation(DateTime absoluteExpiration)
    {
        AbsoluteExpiration = absoluteExpiration.ToUniversalTime();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheInvalidation"/> class.
    /// </summary>
    /// <param name="dependencies">A sequence of <see cref="IDependency"/> implementations that monitors changes in the state of the data which a cache entry depends on. If a state change is registered, the cached value becomes invalid and is removed from the cache.</param>
    public CacheInvalidation(IEnumerable<IDependency> dependencies)
    {
        Dependencies = dependencies ?? Enumerable.Empty<IDependency>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheInvalidation"/> class.
    /// </summary>
    /// <param name="slidingExpiration">The sliding expiration time from when the cached value becomes invalid and is removed from the cache.</param>
    public CacheInvalidation(TimeSpan slidingExpiration)
    {
        Validator.ThrowIfLowerThanOrEqual(slidingExpiration.Ticks, TimeSpan.Zero.Ticks, nameof(slidingExpiration), "The specified sliding expiration cannot be less than or equal to TimeSpan.Zero.");
        Validator.ThrowIfGreaterThan(slidingExpiration.Ticks, TimeSpan.FromDays(365).Ticks, nameof(slidingExpiration), "The specified sliding expiration cannot exceed one year.");
        SlidingExpiration = slidingExpiration;
    }

    /// <summary>
    /// Gets a sequence of objects implementing the <see cref="IDependency"/> interface assigned to a <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>A sequence of objects implementing the <see cref="IDependency"/> interface assigned to a <see cref="CacheEntry"/>.</value>
    public IEnumerable<IDependency> Dependencies { get; }

    /// <summary>
    /// Gets the UTC absolute expiration date time value of a <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The UTC absolute expiration date time value of a <see cref="CacheEntry"/>.</value>
    public DateTime? AbsoluteExpiration { get; }

    /// <summary>
    /// Gets the sliding expiration time of a <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The sliding expiration time of a <see cref="CacheEntry"/>.</value>
    public TimeSpan? SlidingExpiration { get; }

    /// <summary>
    /// Gets a value indicating whether a <see cref="CacheEntry"/> should use <see cref="AbsoluteExpiration"/> property for cache invalidation.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if a <see cref="CacheEntry"/> should use <see cref="AbsoluteExpiration"/> property for cache invalidation; otherwise, <c>false</c>.
    /// </value>
    public bool UseAbsoluteExpiration => AbsoluteExpiration.HasValue;

    /// <summary>
    /// Gets a value indicating whether a <see cref="CacheEntry"/> should use <see cref="SlidingExpiration"/> property for cache invalidation.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if a <see cref="CacheEntry"/> should use <see cref="SlidingExpiration"/> property for cache invalidation; otherwise, <c>false</c>.
    /// </value>
    public bool UseSlidingExpiration => SlidingExpiration.HasValue;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CacheEntry"/> is relying on an <see cref="IDependency"/> implementation for cache invalidation.
    /// </summary>
    /// <value><c>true</c> if a <see cref="CacheEntry"/> is relying on an <see cref="IDependency"/> implementation for cache invalidation; otherwise, <c>false</c>.</value>
    public bool UseDependency => Dependencies != null && Dependencies.Any();
}
