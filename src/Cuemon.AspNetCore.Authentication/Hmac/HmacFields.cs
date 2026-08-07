namespace Cuemon.AspNetCore.Authentication.Hmac;
/// <summary>
/// A collection of constants for <see cref="HmacAuthorizationHeaderBuilder"/> and related.
/// </summary>
public static class HmacFields
{
    /// <summary>
    /// The HTTP request method.
    /// </summary>
    public const string HttpMethod = "httpRequestMethod";

    /// <summary>
    /// The canonical URI that is the URI-encoded version of the absolute path component of an URI.
    /// </summary>
    public const string UriPath = "canonicalUri";

    /// <summary>
    /// The canonical query string.
    /// </summary>
    public const string UriQuery = "canonicalQueryString";

    /// <summary>
    /// The canonical headers.
    /// </summary>
    public const string HttpHeaders = "canonicalHeaders";

    /// <summary>
    /// The delimiter used to separate the key-value pairs of <see cref="HttpHeaders"/>.
    /// </summary>
    public const char HttpHeadersDelimiter = ':';

    /// <summary>
    /// The headers that must be part of the signing process.
    /// </summary>
    public const string SignedHeaders = "signedHeaders";

    /// <summary>
    /// The delimiter used to separate the <see cref="SignedHeaders"/>.
    /// </summary>
    public const char SignedHeadersDelimiter = ';';

    /// <summary>
    /// The request payload.
    /// </summary>
    public const string Payload = "requestPayload";

    /// <summary>
    /// The server date time expressed in ISO 8601 format.
    /// </summary>
    public const string ServerDateTime = "serverDateTime";

    /// <summary>
    /// The public key of the signing process.
    /// </summary>
    public const string ClientId = "clientId";

    /// <summary>
    /// The private key of the signing process.
    /// </summary>
    public const string ClientSecret = "clientSecret";

    /// <summary>
    /// The credential scope that defines the remote resource.
    /// </summary>
    public const string CredentialScope = "credentialScope";

    /// <summary>
    /// The parts of the canonical request.
    /// </summary>
    public const string CanonicalRequest = "canonicalRequest";

    /// <summary>
    /// The parts of the string to sign.
    /// </summary>
    public const string StringToSign = "stringToSign";

    /// <summary>
    /// The default authentication scheme of the <see cref="HmacAuthorizationHeader"/>.
    /// </summary>
    /// <remarks>https://www.wolfe.id.au/2012/10/20/what-is-hmac-authentication-and-why-is-it-useful/, https://docs.microsoft.com/en-us/azure/azure-app-configuration/rest-api-authentication-hmac and https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Authentication.Hmac.HmacAuthorizationHeader.html</remarks>
    public const string Scheme = "HMAC";
}
