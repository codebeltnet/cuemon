namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Configuration options for <see cref="CdnImageTagHelper"/>, <see cref="CdnLinkTagHelper"/> and <see cref="CdnScriptTagHelper"/>.
/// </summary>
public class CdnTagHelperOptions : TagHelperOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CdnTagHelperOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="CdnTagHelperOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="TagHelperOptions.Scheme"/></term>
    ///         <description><see cref="ProtocolUriScheme.Relative"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="TagHelperOptions.BaseUrl"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public CdnTagHelperOptions()
    {
    }
}
