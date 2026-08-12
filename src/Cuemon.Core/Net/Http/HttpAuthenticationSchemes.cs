namespace Cuemon.Net.Http;
/// <summary>
/// Defines constants for well-known HTTP authentication schemes.
/// </summary>
public static class HttpAuthenticationSchemes
{
    /// <summary>
    /// The <c>Basic</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc7617.html and https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader.html</remarks>
    public const string Basic = "Basic";

    /// <summary>
    /// The <c>Bearer</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc6750.html</remarks>
    public const string Bearer = "Bearer";

    /// <summary>
    /// The <c>Digest</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc7616.html and https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader.html</remarks>
    public const string Digest = "Digest";

    /// <summary>
    /// The <c>HOBA</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc7486.html</remarks>
    public const string Hoba = "HOBA";

    /// <summary>
    /// The <c>Mutual</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc8120.html</remarks>
    public const string Mutual = "Mutual";

    /// <summary>
    /// The <c>Negotiate</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc4559.html</remarks>
    public const string Negotiate = "Negotiate";

    /// <summary>
    /// The <c>SCRAM-SHA-1</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc7804.html</remarks>
    public const string ScramSha1 = "SCRAM-SHA-1";

    /// <summary>
    /// The <c>SCRAM-SHA-256</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc7804.html</remarks>
    public const string ScramSha256 = "SCRAM-SHA-256";

    /// <summary>
    /// The <c>vapid</c> HTTP Authentication Scheme.
    /// </summary>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc8292.html</remarks>
    public const string Vapid = "vapid";
}
