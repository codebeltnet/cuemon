using System;

namespace Cuemon.AspNetCore.Configuration;
/// <summary>
/// Specifies options that is related to <see cref="DynamicCacheBusting"/> operations.
/// </summary>
/// <seealso cref="CacheBustingOptions" />
public class DynamicCacheBustingOptions : CacheBustingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicCacheBustingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="DynamicCacheBustingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="PreferredLength"/></term>
    ///         <description>8</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PreferredCharacters"/></term>
    ///         <description><see cref="Alphanumeric.LettersAndNumbers"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="TimeToLive"/></term>
    ///         <description>12 hours</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public DynamicCacheBustingOptions()
    {
        TimeToLive = TimeSpan.FromHours(12);
        PreferredLength = 8;
        PreferredCharacters = Alphanumeric.LettersAndNumbers;
    }

    /// <summary>
    /// Gets or sets the preferred length of <see cref="CacheBusting.Version"/>.
    /// </summary>
    /// <value>The preferred length of <see cref="CacheBusting.Version"/>.</value>
    public int PreferredLength { get; set; }

    /// <summary>
    /// Gets or sets the preferred characters of <see cref="CacheBusting.Version"/>.
    /// </summary>
    /// <value>The preferred characters of <see cref="CacheBusting.Version"/>.</value>
    public string PreferredCharacters { get; set; }

    /// <summary>
    /// Gets or sets the TTL of <see cref="CacheBusting.Version"/>.
    /// </summary>
    /// <value>The TTL of <see cref="CacheBusting.Version"/>.</value>
    public TimeSpan TimeToLive { get; set; }
}
