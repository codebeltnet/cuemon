using System;
using Cuemon.Configuration;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Configuration;
/// <summary>
/// Provides cache-busting capabilities on a duration based interval. This class cannot be inherited.
/// </summary>
/// <seealso cref="CacheBusting" />
public sealed class DynamicCacheBusting : CacheBusting, IConfigurable<DynamicCacheBustingOptions>
{
    private string _version;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicCacheBusting"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="DynamicCacheBustingOptions"/> which need to be configured.</param>
    public DynamicCacheBusting(IOptions<DynamicCacheBustingOptions> setup)
    {
        Options = setup.Value;
    }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public DynamicCacheBustingOptions Options { get; }

    /// <summary>
    /// Gets the UTC timestamp from when the <see cref="CacheBusting.Version"/> was last changed.
    /// </summary>
    /// <value>The UTC timestamp from when the <see cref="CacheBusting.Version"/> was last changed.</value>
    public DateTime UtcChanged { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the version to be a part of the link you need cache-busting compatible.
    /// </summary>
    /// <value>The version to be a part of the link you need cache-busting compatible.</value>
    public override string Version
    {
        get
        {
            var utcNow = DateTime.UtcNow;
            var range = new DateTimeRange(UtcChanged, utcNow);
            if (string.IsNullOrEmpty(_version) || range.Duration >= Options.TimeToLive)
            {
                _version = Decorator.Enclose(Generate.RandomString(Decorator.Enclose(Options.PreferredLength).Max(6), Options.PreferredCharacters)).ToCasing(Options.PreferredCasing);
                UtcChanged = utcNow;
            }
            return _version;
        }
    }
}
