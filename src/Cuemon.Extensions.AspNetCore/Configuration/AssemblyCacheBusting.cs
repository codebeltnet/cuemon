using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Extensions.Data.Integrity;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Configuration;
/// <summary>
/// Provides cache-busting capabilities from an Assembly. This class cannot be inherited.
/// </summary>
/// <seealso cref="CacheBusting" />
public sealed class AssemblyCacheBusting : CacheBusting
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyCacheBusting"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="AssemblyCacheBustingOptions"/> which need to be configured.</param>
    public AssemblyCacheBusting(IOptions<AssemblyCacheBustingOptions> setup)
    {
        var options = setup.Value;
        var version = options.Assembly?.GetCacheValidator(() => UnkeyedHashFactory.CreateCrypto(options.Algorithm), o =>
        {
            if (options.ReadByteForByteChecksum) { o.BytesToRead = int.MaxValue; }
        }).Checksum.ToHexadecimalString() ?? Guid.NewGuid().ToString("N"); // fallback to Guid.NewGuid
        Version = Decorator.Enclose(version).ToCasing(options.PreferredCasing);
    }

    /// <summary>
    /// Gets the version to be a part of the link you need cache-busting compatible.
    /// </summary>
    /// <value>The version to be a part of the link you need cache-busting compatible.</value>
    public override string Version { get; }
}
