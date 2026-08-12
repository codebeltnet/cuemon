namespace Cuemon.Net.Http;
/// <summary>
/// Defines constants for well-known HTTP headers.
/// </summary>
/// <remarks>This is primarily a statement to the (IMO) bad design decision that was made effective from .NET Core 3.0 and forward. Discussion: https://github.com/dotnet/aspnetcore/issues/9514</remarks>
public static class HttpHeaderNames
{
    #region Request Headers
    /// <summary>
    /// The <c>Accept</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept</remarks>
    public const string Accept = "Accept";

    /// <summary>
    /// The <c>Accept-Charset</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Charset</remarks>
    public const string AcceptCharset = "Accept-Charset";

    /// <summary>
    /// The <c>Accept-Encoding</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Encoding</remarks>
    public const string AcceptEncoding = "Accept-Encoding";

    /// <summary>
    /// The <c>Accept-Language</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language</remarks>
    public const string AcceptLanguage = "Accept-Language";

    /// <summary>
    /// The <c>Access-Control-Request-Headers</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Request-Headers</remarks>
    public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";

    /// <summary>
    /// The <c>Access-Control-Request-Method</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Request-Method</remarks>
    public const string AccessControlRequestMethod = "Access-Control-Request-Method";

    /// <summary>
    /// The <c>Authorization</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization</remarks>
    public const string Authorization = "Authorization";

    /// <summary>
    /// The <c>Cache-Control</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cache-Control</remarks>
    public const string CacheControl = "Cache-Control";

    /// <summary>
    /// The <c>Connection</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Connection</remarks>
    public const string Connection = "Connection";

    /// <summary>
    /// The <c>Content-Encoding</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Encoding</remarks>
    public const string ContentEncoding = "Content-Encoding";

    /// <summary>
    /// The <c>Content-Length</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Length</remarks>
    public const string ContentLength = "Content-Length";

    /// <summary>
    /// The <c>Content-MD5</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-MD5</remarks>
    public const string ContentMD5 = "Content-MD5";

    /// <summary>
    /// The <c>Content-Type</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type</remarks>
    public const string ContentType = "Content-Type";

    /// <summary>
    /// The <c>Cookie</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cookie</remarks>
    public const string Cookie = "Cookie";

    /// <summary>
    /// The <c>Date</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Date</remarks>
    public const string Date = "Date";

    /// <summary>
    /// The <c>Expect</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect</remarks>
    public const string Expect = "Expect";

    /// <summary>
    /// The <c>Forwarded</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Forwarded</remarks>
    public const string Forwarded = "Forwarded";

    /// <summary>
    /// The <c>From</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/From</remarks>
    public const string From = "From";

    /// <summary>
    /// The <c>Host</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Host</remarks>
    public const string Host = "Host";

    /// <summary>
    /// The <c>HTTP2-Settings</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/HTTP2-Settings</remarks>
    public const string Http2Settings = "HTTP2-Settings";

    /// <summary>
    /// The <c>If-Match</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Match</remarks>
    public const string IfMatch = "If-Match";

    /// <summary>
    /// The <c>If-Modified-Since</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Modified-Since</remarks>
    public const string IfModifiedSince = "If-Modified-Since";

    /// <summary>
    /// The <c>If-None-Match</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-None-Match</remarks>
    public const string IfNoneMatch = "If-None-Match";

    /// <summary>
    /// The <c>If-Range</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Range</remarks>
    public const string IfRange = "If-Range";

    /// <summary>
    /// The <c>If-Unmodified-Since</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Unmodified-Since</remarks>
    public const string IfUnmodifiedSince = "If-Unmodified-Since";

    /// <summary>
    /// The <c>Max-Forwards</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Max-Forwards</remarks>
    public const string MaxForwards = "Max-Forwards";

    /// <summary>
    /// The <c>Origin</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Origin</remarks>
    public const string Origin = "Origin";

    /// <summary>
    /// The <c>Pragma</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Pragma</remarks>
    public const string Pragma = "Pragma";

    /// <summary>
    /// The <c>Prefer</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Prefer</remarks>
    public const string Prefer = "Prefer";

    /// <summary>
    /// The <c>Proxy-Authorization</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Proxy-Authorization</remarks>
    public const string ProxyAuthorization = "Proxy-Authorization";

    /// <summary>
    /// The <c>Range</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Range</remarks>
    public const string Range = "Range";

    /// <summary>
    /// The <c>Referer</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referer</remarks>
    public const string Referer = "Referer";

    /// <summary>
    /// The <c>TE</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/TE</remarks>
    public const string TE = "TE";

    /// <summary>
    /// The <c>Trailer</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Trailer</remarks>
    public const string Trailer = "Trailer";

    /// <summary>
    /// The <c>Transfer-Encoding</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Transfer-Encoding</remarks>
    public const string TransferEncoding = "Transfer-Encoding";

    /// <summary>
    /// The <c>User-Agent</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/User-Agent</remarks>
    public const string UserAgent = "User-Agent";

    /// <summary>
    /// The <c>Upgrade</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Upgrade</remarks>
    public const string Upgrade = "Upgrade";

    /// <summary>
    /// The <c>Via</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Via</remarks>
    public const string Via = "Via";

    /// <summary>
    /// The <c>Warning</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Warning</remarks>
    public const string Warning = "Warning";

    /// <summary>
    /// The de facto standard <c>X-CSRF-Token</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Cross-site_request_forgery#Cookie-to-header_token</remarks>
    public const string XCsrfToken = "X-CSRF-Token";

    /// <summary>
    /// The de facto standard <c>X-Correlation-ID</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://www.rapid7.com/blog/post/2016/12/23/the-value-of-correlation-ids/</remarks>
    public const string XCorrelationId = "X-Correlation-ID";

    /// <summary>
    /// The de facto standard <c>X-Forwarded-For</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-For</remarks>
    public const string XForwardedFor = "X-Forwarded-For";

    /// <summary>
    /// The de facto standard <c>X-Forwarded-Host</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Host</remarks>
    public const string XForwardedHost = "X-Forwarded-Host";

    /// <summary>
    /// The de facto standard <c>X-Forwarded-Proto</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Proto</remarks>
    public const string XForwardedProto = "X-Forwarded-Proto";

    /// <summary>
    /// The de facto standard <c>X-Request-ID</c> request HTTP header name.
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/56068619/should-i-use-request-id-x-request-id-or-x-correlation-id-in-the-request-header</remarks>
    public const string XRequestId = "X-Request-ID";

    /// <summary>
    /// The de facto standard <c>X-Api-Key</c> request HTTP header name.
    /// </summary>
    /// <remarks>
    /// X-Api-Key should not be used as authentication/authorization; it is merely a convenient first-line-of-defense in protecting your APIs.
    /// Any use of X-Api-Key in this framework will result in 403 Forbidden - and not 401 Unauthorized; for 401 you should use well-known authentication schemes. 
    /// Further info: https://aws.amazon.com/premiumsupport/knowledge-center/api-gateway-troubleshoot-403-forbidden/
    /// </remarks>
    public const string XApiKey = "X-Api-Key";
    #endregion

    #region Response Headers
    /// <summary>
    /// The <c>Accept-Ranges</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Ranges</remarks>
    public const string AcceptRanges = "Accept-Ranges";

    /// <summary>
    /// The <c>Access-Control-Allow-Credentials</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Credentials</remarks>
    public const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

    /// <summary>
    /// The <c>Access-Control-Allow-Headers</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Headers</remarks>
    public const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

    /// <summary>
    /// The <c>Access-Control-Allow-Methods</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Methods</remarks>
    public const string AccessControlAllowMethods = "Access-Control-Allow-Methods";

    /// <summary>
    /// The <c>Access-Control-Allow-Origin</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin</remarks>
    public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

    /// <summary>
    /// The <c>Access-Control-Expose-Headers</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Expose-Headers</remarks>
    public const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";

    /// <summary>
    /// The <c>Access-Control-Max-Age</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Max-Age</remarks>
    public const string AccessControlMaxAge = "Access-Control-Max-Age";

    /// <summary>
    /// The <c>Age</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Age</remarks>
    public const string Age = "Age";

    /// <summary>
    /// The <c>Allow</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Allow</remarks>
    public const string Allow = "Allow";

    /// <summary>
    /// The <c>Content-Disposition</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition</remarks>
    public const string ContentDisposition = "Content-Disposition";

    /// <summary>
    /// The <c>Content-Language</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Language</remarks>
    public const string ContentLanguage = "Content-Language";

    /// <summary>
    /// The <c>Content-Location</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Location</remarks>
    public const string ContentLocation = "Content-Location";

    /// <summary>
    /// The <c>Content-Range</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Range</remarks>
    public const string ContentRange = "Content-Range";

    /// <summary>
    /// The <c>Content-Security-Policy</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy</remarks>
    public const string ContentSecurityPolicy = "Content-Security-Policy";

    /// <summary>
    /// The <c>Content-Security-Policy-Report-Only</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy-Report-Only</remarks>
    public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";

    /// <summary>
    /// The <c>ETag</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag</remarks>
    public const string ETag = "ETag";

    /// <summary>
    /// The <c>Expires</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expires</remarks>
    public const string Expires = "Expires";

    /// <summary>
    /// The <c>Last-Modified</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Last-Modified</remarks>
    public const string LastModified = "Last-Modified";

    /// <summary>
    /// The <c>Location</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Location</remarks>
    public const string Location = "Location";

    /// <summary>
    /// The <c>Proxy-Authenticate</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Proxy-Authenticate</remarks>
    public const string ProxyAuthenticate = "Proxy-Authenticate";

    /// <summary>
    /// The <c>Retry-After</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Retry-After</remarks>
    public const string RetryAfter = "Retry-After";

    /// <summary>
    /// The <c>Server</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Server</remarks>
    public const string Server = "Server";

    /// <summary>
    /// The <c>Server-Timing</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Server-Timing</remarks>
    public const string ServerTiming = "Server-Timing";

    /// <summary>
    /// The <c>Set-Cookie</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie</remarks>
    public const string SetCookie = "Set-Cookie";

    /// <summary>
    /// The <c>Strict-Transport-Security</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security</remarks>
    public const string StrictTransportSecurity = "Strict-Transport-Security";

    /// <summary>
    /// The <c>Vary</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Vary</remarks>
    public const string Vary = "Vary";

    /// <summary>
    /// The <c>WWW-Authenticate</c> response HTTP header name.
    /// </summary>
    /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/WWW-Authenticate</remarks>
    public const string WWWAuthenticate = "WWW-Authenticate";
    #endregion
}
