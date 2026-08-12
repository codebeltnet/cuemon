namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// A collection of constants for <see cref="DigestAuthorizationHeaderBuilder"/>.
/// </summary>
public static class DigestFields
{
    /// <summary>
    /// The username field of a HTTP Digest access authentication.
    /// </summary>
    public const string UserName = "username";

    /// <summary>
    /// The realm field of a HTTP Digest access authentication.
    /// </summary>
    public const string Realm = "realm";

    /// <summary>
    /// The response field of a HTTP Digest access authentication.
    /// </summary>
    public const string Response = "response";

    /// <summary>
    /// The qop (quality of protection) field of a HTTP Digest access authentication.
    /// </summary>
    public const string QualityOfProtection = "qop";

    /// <summary>
    /// The client nonce (cnonce) field of a HTTP Digest access authentication.
    /// </summary>
    public const string ClientNonce = "cnonce";

    /// <summary>
    /// The nc (nonce count) field of a HTTP Digest access authentication.
    /// </summary>
    public const string NonceCount = "nc";

    /// <summary>
    /// The nonce field of a HTTP Digest access authentication.
    /// </summary>
    public const string Nonce = "nonce";

    /// <summary>
    /// The uri (digest URI) field of a HTTP Digest access authentication.
    /// </summary>
    public const string DigestUri = "uri";

    /// <summary>
    /// The opaque field of a HTTP Digest access authentication.
    /// </summary>
    public const string Opaque = "opaque";

    /// <summary>
    /// The algorithm field of a HTTP Digest access authentication.
    /// </summary>
    public const string Algorithm = "algorithm";

    /// <summary>
    /// The stale field of a HTTP Digest access authentication.
    /// </summary>
    public const string Stale = "stale";
}
