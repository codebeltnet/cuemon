namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Defines protocol URI schemes for static resource related operations.
/// </summary>
public enum ProtocolUriScheme
{
    /// <summary>
    /// Specifies that the URI scheme is not defined.
    /// </summary>
    None,
    /// <summary>
    /// Specifies that the URI scheme is protocol-relative (//).
    /// </summary>
    Relative,
    /// <summary>
    /// Specifies that the URI scheme is Hypertext Transfer Protocol (HTTP).
    /// </summary>
    Http,
    /// <summary>
    /// Specifies that the URI scheme is Secure Hypertext Transfer Protocol (HTTPS).
    /// </summary>
    Https
}
