using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Configuration;
/// <summary>
/// Specifies options that is related to <see cref="CacheBusting"/> operations.
/// </summary>
public class CacheBustingOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheBustingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="CacheBustingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="PreferredCasing"/></term>
    ///         <description><see cref="CasingMethod.LowerCase"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public CacheBustingOptions()
    {
        PreferredCasing = CasingMethod.LowerCase;
    }

    /// <summary>
    /// Gets or sets the preferred casing to use on <see cref="CacheBusting.Version"/>.
    /// </summary>
    /// <value>The preferred casing to use on <see cref="CacheBusting.Version"/>.</value>
    public CasingMethod PreferredCasing { get; set; }
}
