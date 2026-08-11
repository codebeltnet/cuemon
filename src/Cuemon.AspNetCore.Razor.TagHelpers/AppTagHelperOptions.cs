namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Configuration options for <see cref="AppImageTagHelper"/>, <see cref="AppLinkTagHelper"/> and <see cref="AppScriptTagHelper"/>.
/// </summary>
public class AppTagHelperOptions : TagHelperOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppTagHelperOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AppTagHelperOptions"/>.
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
    ///         <term><see cref="TagHelperOptions.BaseUrlMode"/></term>
    ///         <description><see cref="TagHelperBaseUrlMode.Configured"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="TagHelperOptions.BaseUrl"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AppTagHelperOptions()
    {
    }
}
