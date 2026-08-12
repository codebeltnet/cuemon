using System;
using Cuemon.Configuration;

namespace Cuemon.Runtime.Caching;
/// <summary>
/// Configuration options for <see cref="SlimMemoryCache"/>.
/// </summary>
public class SlimMemoryCacheOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SlimMemoryCacheOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="SlimMemoryCacheOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="EnableCleanup"/></term>
    ///         <description><c>true</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="FirstSweep"/></term>
    ///         <description>After 30 seconds</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SucceedingSweep"/></term>
    ///         <description>Every 2 minutes</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="KeyProvider"/></term>
    ///         <description><c>(key, ns) => Generate.HashCode64(ns == Cache.NoScope ? key.ToUpperInvariant() : $"{key}^{nameof(SlimMemoryCache)}^{ns}".ToUpperInvariant());</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public SlimMemoryCacheOptions()
    {
        EnableCleanup = true;
        FirstSweep = TimeSpan.FromSeconds(30);
        SucceedingSweep = TimeSpan.FromMinutes(2);
        KeyProvider = (key, ns) => Generate.HashCode64(ns == CacheEntry.NoScope ? key.ToUpperInvariant() : $"{key}^{nameof(SlimMemoryCache)}^{ns}".ToUpperInvariant());
    }

    /// <summary>
    /// Gets or sets a value indicating whether a periodic sweep clean-up is done on the cache.
    /// </summary>
    /// <value><c>true</c> if a periodic sweep clean-up is done on the cache; otherwise, <c>false</c>.</value>
    public bool EnableCleanup { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="TimeSpan"/> that specifies the amount of time to wait before the initial first sweep clean-up.
    /// </summary>
    /// <value>The <see cref="TimeSpan"/> that specifies the amount of time to wait before the initial first sweep clean-up.</value>
    public TimeSpan FirstSweep { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="TimeSpan"/> that specifies the interval for every succeeding sweep clean-up after the initial <see cref="FirstSweep"/>.
    /// </summary>
    /// <value>The <see cref="TimeSpan"/> that specifies the interval for every succeeding sweep clean-up.</value>
    public TimeSpan SucceedingSweep { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that is responsible for providing a unique identifier for a cache entry.
    /// </summary>
    /// <value>The function delegate that is responsible for providing a unique identifier for a cache entry.</value>
    public Func<string, string, long> KeyProvider { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="KeyProvider"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(KeyProvider == null);
    }
}
