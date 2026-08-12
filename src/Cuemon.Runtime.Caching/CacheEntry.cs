using System;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Represents an individual cache entry in the cache.
/// </summary>
public class CacheEntry
{
    /// <summary>
    /// Represents a cache with a global scope, eg. no namespace.
    /// </summary>
    public const string NoScope = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheEntry"/> class.
    /// </summary>
    /// <param name="key">The unique identifier of the cache.</param>
    /// <param name="value">The stored value of the cache.</param>
    /// <param name="ns">The optional namespace that provides a scope to the cache.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public CacheEntry(string key, object value, string ns = NoScope)
    {
        Validator.ThrowIfNull(key);
        var timestamp = DateTime.UtcNow;
        Key = key;
        Value = value;
        Namespace = ns;
        Inserted = timestamp;
        Accessed = timestamp;
    }

    /// <summary>
    /// Occurs when a <see cref="CacheEntry"/> object with an associated <see cref="Dependency"/> has expired.
    /// </summary>
    public event EventHandler<CacheEntryEventArgs> Expired;

    /// <summary>
    /// Gets the unique identifier of this <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The unique identifier of this <see cref="CacheEntry"/>.</value>
    public string Key { get; }

    /// <summary>
    /// Gets the stored value of this <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The stored value of this <see cref="CacheEntry"/>.</value>
    public object Value { get; set; }

    /// <summary>
    /// Gets the optional namespace that provides a scope to this <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The optional namespace that provides a scope to this <see cref="CacheEntry"/>.</value>
    public string Namespace { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Generate.ObjectPortrayal(this, o => o.BypassOverrideCheck = true);
    }

    /// <summary>
    /// Gets the cache invalidation of this <see cref="CacheEntry"/>.
    /// </summary>
    /// <value>The cache invalidation of this <see cref="CacheEntry"/>.</value>
    public CacheInvalidation Invalidation { get; private set; }

    /// <summary>
    /// Gets the UTC date time value from when this <see cref="CacheEntry"/> was inserted.
    /// </summary>
    /// <value>The UTC date time value from when this <see cref="CacheEntry"/> was inserted.</value>
    public DateTime Inserted { get; }

    /// <summary>
    /// Gets the UTC date time value from when this <see cref="CacheEntry"/> was last accessed.
    /// </summary>
    /// <value>The UTC date time value from when this <see cref="CacheEntry"/> was last accessed.</value>
    public DateTime Accessed { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CacheEntry"/> can expire.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this <see cref="CacheEntry"/> can expire; otherwise, <c>false</c>.
    /// </value>
    public bool CanExpire => Invalidation.UseAbsoluteExpiration || Invalidation.UseSlidingExpiration || Invalidation.UseDependency;

    internal CacheEntry SetInvalidation(CacheInvalidation invalidation)
    {
        Validator.ThrowIfNull(invalidation);
        Invalidation = invalidation;
        return this;
    }

    /// <summary>
    /// Determines whether the specified time resolves this <see cref="CacheEntry"/> as expired.
    /// </summary>
    /// <param name="time">The date and time to evaluate against.</param>
    /// <returns>
    /// 	<c>true</c> if the specified time resolves this <see cref="CacheEntry"/> as expired; otherwise, <c>false</c>.
    /// </returns>
    public bool HasExpired(DateTime time)
    {
        if (!CanExpire) { return false; }
        if (Invalidation.UseAbsoluteExpiration)
        {
            if (time >= Invalidation.AbsoluteExpiration) { return true; }
        }
        else if (Invalidation.UseSlidingExpiration)
        {
            var currentPeriod = (time - Accessed);
            if (currentPeriod >= Invalidation.SlidingExpiration) { return true; }
        }
        else if (Invalidation.UseDependency)
        {
            foreach (var dependency in Invalidation.Dependencies)
            {
                if (dependency.HasChanged) { return true; }
            }
        }
        return false;
    }

    internal void Refresh()
    {
        Accessed = DateTime.UtcNow;
    }

    internal CacheEntry StartDependencies()
    {
        if (Invalidation.UseDependency)
        {
            foreach (var dependency in Invalidation.Dependencies)
            {
                dependency.DependencyChanged += ProcessDependencyChanged;
                dependency.Start();
            }
        }
        return this;
    }

    private void ProcessDependencyChanged(object sender, DependencyEventArgs e)
    {
        if (Invalidation.UseDependency)
        {
            OnExpiredRaised(new CacheEntryEventArgs(this));
            foreach (var dependency in Invalidation.Dependencies)
            {
                dependency.DependencyChanged -= ProcessDependencyChanged;
            }
        }
    }

    /// <summary>
    /// Raises the <see cref="Expired"/> event.
    /// </summary>
    /// <param name="e">The <see cref="CacheEntryEventArgs"/> instance containing the event data.</param>
    protected virtual void OnExpiredRaised(CacheEntryEventArgs e)
    {
        Expired?.Invoke(this, e);
    }
}
