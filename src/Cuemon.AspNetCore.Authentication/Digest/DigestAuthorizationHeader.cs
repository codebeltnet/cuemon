using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Provides a representation of a HTTP Digest Access Authentication header.
/// Implements the <see cref="AuthorizationHeader" />
/// </summary>
/// <seealso cref="AuthorizationHeader" />
public class DigestAuthorizationHeader : AuthorizationHeader
{
    /// <summary>
    /// Creates an instance of <see cref="DigestAuthorizationHeader"/> from the specified parameters.
    /// </summary>
    /// <param name="authorizationHeader">The raw HTTP authorization header.</param>
    /// <returns>An instance of <see cref="DigestAuthorizationHeader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="authorizationHeader"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="authorizationHeader"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static DigestAuthorizationHeader Create(string authorizationHeader)
    {
        Validator.ThrowIfNullOrWhitespace(authorizationHeader);
        return new DigestAuthorizationHeader().Parse(authorizationHeader, o => o.CredentialsDelimiter = ", ") as DigestAuthorizationHeader;
    }

    /// <summary>
    /// The authentication scheme of the <see cref="DigestAuthorizationHeader"/>.
    /// </summary>
    public const string Scheme = HttpAuthenticationSchemes.Digest;

    private DigestAuthorizationHeader() : base(Scheme)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthorizationHeader"/> class.
    /// </summary>
    /// <param name="realm">The realm/credential scope that defines the remote resource.</param>
    /// <param name="nonce">The unique server generated string.</param>
    /// <param name="opaque">The string of data specified by the server.</param>
    /// <param name="algorithm">The algorithm used to produce the digest and an unkeyed digest.</param>
    /// <param name="userName">The username of the specified <paramref name="realm"/>.</param>
    /// <param name="uri">The effective request URI.</param>
    /// <param name="nc">The hexadecimal count of the number of requests the client has sent with the <paramref name="nonce"/> value.</param>
    /// <param name="cNonce">The unique client generated string.</param>
    /// <param name="qop">The "quality of protection" the client has applied to the message.</param>
    /// <param name="response">The computed response which proves that the user knows a password.</param>
    public DigestAuthorizationHeader(string realm, string nonce, string opaque, string algorithm, string userName, string uri, string nc, string cNonce, string qop, string response) : base(Scheme)
    {
        Realm = realm;
        Nonce = nonce;
        Opaque = opaque;
        Algorithm = algorithm;
        UserName = userName;
        Uri = uri;
        NC = nc;
        CNonce = cNonce;
        Qop = qop;
        Response = response;
    }

    /// <summary>
    /// Gets the realm/credential scope that defines the remote resource.
    /// </summary>
    /// <value>The realm/credential scope that defines the remote resource.</value>
    public string Realm { get; }

    /// <summary>
    /// Gets the unique server generated string.
    /// </summary>
    /// <value>The unique server generated string.</value>
    public string Nonce { get; }

    /// <summary>
    /// Gets the string of data specified by the server.
    /// </summary>
    /// <value>The string of data specified by the server.</value>
    public string Opaque { get; }

    /// <summary>
    /// Gets the algorithm used to produce the digest and an unkeyed digest.
    /// </summary>
    /// <value>The algorithm used to produce the digest and an unkeyed digest.</value>
    public string Algorithm { get; }

    /// <summary>
    /// Gets the username of the specified <see cref="Realm"/>.
    /// </summary>
    /// <value>The username of the specified <see cref="Realm"/>.</value>
    public string UserName { get; }

    /// <summary>
    /// Gets the effective request URI.
    /// </summary>
    /// <value>The effective request URI.</value>
    public string Uri { get; }

    /// <summary>
    /// Gets the computed response which proves that the user knows a password.
    /// </summary>
    /// <value>The computed response which proves that the user knows a password.</value>
    public string Response { get; }

    /// <summary>
    /// Gets the "quality of protection" the client has applied to the message.
    /// </summary>
    /// <value>The "quality of protection" the client has applied to the message.</value>
    public string Qop { get; }

    /// <summary>
    /// Gets the unique client generated string.
    /// </summary>
    /// <value>The unique client generated string.</value>
    public string CNonce { get; }

    /// <summary>
    /// Gets the hexadecimal count of the number of requests the client has sent with the <see cref="Nonce"/> value.
    /// </summary>
    /// <value>The hexadecimal count of the number of requests the client has sent with the <see cref="Nonce"/> value.</value>
    public string NC { get; }

    /// <summary>
    /// The core parser that resolves an <see cref="AuthorizationHeader" /> from a set of <paramref name="credentials" />.
    /// </summary>
    /// <param name="credentials">The credentials used in authentication.</param>
    /// <returns>An <see cref="AuthorizationHeader" /> equivalent of <paramref name="credentials" />.</returns>
    protected override AuthorizationHeader ParseCore(IReadOnlyDictionary<string, string> credentials)
    {
        var valid = credentials.TryGetValue(DigestFields.Realm, out var realm);
        valid |= credentials.TryGetValue(DigestFields.Nonce, out var nonce);
        valid |= credentials.TryGetValue(DigestFields.Opaque, out var opaque);
        valid |= credentials.TryGetValue(DigestFields.Algorithm, out var algorithm);
        valid |= credentials.TryGetValue(DigestFields.UserName, out var userName);
        valid |= credentials.TryGetValue(DigestFields.DigestUri, out var uri);
        valid |= credentials.TryGetValue(DigestFields.Response, out var response);
        valid |= credentials.TryGetValue(DigestFields.QualityOfProtection, out var qop);
        valid |= credentials.TryGetValue(DigestFields.ClientNonce, out var cnonce);
        valid |= credentials.TryGetValue(DigestFields.NonceCount, out var nc);
        return valid ? new DigestAuthorizationHeader(realm, nonce, opaque, algorithm, userName, uri, nc, cnonce, qop, response) : null;
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(AuthenticationScheme);
        AppendField(sb, DigestFields.UserName, UserName);
        AppendField(sb, DigestFields.Realm, Realm);
        AppendField(sb, DigestFields.Nonce, Nonce);
        AppendField(sb, DigestFields.DigestUri, Uri);
        AppendField(sb, DigestFields.QualityOfProtection, Qop, false);
        AppendField(sb, DigestFields.NonceCount, NC, false);
        AppendField(sb, DigestFields.ClientNonce, CNonce);
        AppendField(sb, DigestFields.Response, Response);
        AppendField(sb, DigestFields.Opaque, Opaque);
        AppendField(sb, DigestFields.Algorithm, Algorithm, false);
        return sb.ToString().TrimEnd(',');
    }

    private static void AppendField(StringBuilder sb, string fn, string fv, bool useQuotedStringSyntax = true)
    {
        if (!string.IsNullOrWhiteSpace(fv)) { sb.Append(CultureInfo.InvariantCulture, $" {fn}={Parse(fv, useQuotedStringSyntax)},"); }
    }

    private static string Parse(string value, bool useQuotedStringSyntax)
    {
        return useQuotedStringSyntax
            ? $"\"{value}\""
            : value;
    }
}
