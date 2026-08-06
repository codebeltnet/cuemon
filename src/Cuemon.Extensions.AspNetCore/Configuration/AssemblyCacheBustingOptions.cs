using System.Reflection;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;

namespace Cuemon.Extensions.AspNetCore.Configuration;
/// <summary>
/// Specifies options that is related to <see cref="AssemblyCacheBustingOptions"/> operations.
/// </summary>
/// <seealso cref="CacheBustingOptions" />
public class AssemblyCacheBustingOptions : CacheBustingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyCacheBustingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="DynamicCacheBustingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Assembly"/></term>
    ///         <description><see cref="Assembly.GetEntryAssembly"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Algorithm"/></term>
    ///         <description><see cref="UnkeyedCryptoAlgorithm.Md5"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ReadByteForByteChecksum"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AssemblyCacheBustingOptions()
    {
        Assembly = Assembly.GetEntryAssembly();
        Algorithm = UnkeyedCryptoAlgorithm.Md5;
        ReadByteForByteChecksum = false;
    }

    /// <summary>
    /// Gets or sets the assembly that should be used as reference for the cache-busting.
    /// </summary>
    /// <value>The assembly that should be used as reference for the cache-busting.</value>
    public Assembly Assembly { get; set; }

    /// <summary>
    /// Gets or sets the hash algorithm to use for the computation of <see cref="Assembly"/>.
    /// </summary>
    /// <value>The hash algorithm to use for the computation of <see cref="Assembly"/>.</value>
    public UnkeyedCryptoAlgorithm Algorithm { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Assembly"/> will be read byte-for-byte when computing the checksum.
    /// </summary>
    /// <value><c>true</c> if the <see cref="Assembly"/> will be read byte-for-byte when computing the checksum; otherwise, <c>false</c>.</value>
    public bool ReadByteForByteChecksum { get; set; }
}
