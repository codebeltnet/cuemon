using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Authentication.Hmac;
/// <summary>
/// Provides a representation of a HTTP HMAC Authentication header.
/// Implements the <see cref="AuthorizationHeader" />
/// </summary>
/// <seealso cref="AuthorizationHeader" />
public class HmacAuthorizationHeader : AuthorizationHeader
{
    private const string CredentialComponent = "Credential";
    private const string SignedHeadersComponent = "SignedHeaders";
    private const string SignatureComponent = "Signature";

    /// <summary>
    /// Creates an instance of <see cref="HmacAuthorizationHeader"/> from the specified parameters.
    /// </summary>
    /// <param name="authenticationScheme">The name of the authentication scheme.</param>
    /// <param name="authorizationHeader">The raw HTTP authorization header.</param>
    /// <param name="setup">The <see cref="AuthorizationHeaderOptions" /> which may be configured.</param>
    /// <returns>An instance of <see cref="HmacAuthorizationHeader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="authenticationScheme"/> cannot be null -or-
    /// <paramref name="authorizationHeader"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="authenticationScheme"/> cannot be empty or consist only of white-space characters -or-
    /// <paramref name="authorizationHeader"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static HmacAuthorizationHeader Create(string authenticationScheme, string authorizationHeader, Action<AuthorizationHeaderOptions> setup = null)
    {
        Validator.ThrowIfNullOrWhitespace(authenticationScheme);
        Validator.ThrowIfNullOrWhitespace(authorizationHeader);
        return new HmacAuthorizationHeader(authenticationScheme).Parse(authorizationHeader, setup) as HmacAuthorizationHeader;
    }

    private HmacAuthorizationHeader(string authenticationScheme) : base(authenticationScheme)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HmacAuthorizationHeader"/> class.
    /// </summary>
    /// <param name="clientId">The client identifier that is the public key of the signing process.</param>
    /// <param name="credentialScope">The credential scope that defines the remote resource.</param>
    /// <param name="signedHeaders">The headers that will be part of the signing process.</param>
    /// <param name="signature">The signature that represents the integrity of this header.</param>
    /// <param name="authenticationScheme">The authentication scheme of this header. Default is <see cref="HmacFields.Scheme"/> (HMAC).</param>
    public HmacAuthorizationHeader(string clientId, string credentialScope, string signedHeaders, string signature, string authenticationScheme = HmacFields.Scheme) : base(authenticationScheme)
    {
        ClientId = clientId;
        CredentialScope = credentialScope;
        SignedHeaders = signedHeaders.Split(';');
        Signature = signature;
    }

    /// <summary>
    /// Gets the client identifier that is the public key of the signing process.
    /// </summary>
    /// <value>The client identifier that is the public key of the signing process.</value>
    public string ClientId { get; }

    /// <summary>
    /// Gets the credential scope that defines the remote resource.
    /// </summary>
    /// <value>The credential scope that defines the remote resource.</value>
    public string CredentialScope { get; }

    /// <summary>
    /// Gets the headers that will be part of the signing process.
    /// </summary>
    /// <value>The headers that will be part of the signing process.</value>
    public string[] SignedHeaders { get; }

    /// <summary>
    /// Gets the signature that represents the integrity of this header.
    /// </summary>
    /// <value>The signature that represents the integrity of this header.</value>
    public string Signature { get; }

    private static readonly char[] ForwardSlashSeparator = new[] { '/' };

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return $"{AuthenticationScheme} {CredentialComponent}={ClientId}/{CredentialScope}, {SignedHeadersComponent}={string.Join(";", SignedHeaders)}, {SignatureComponent}={Signature}";
    }

    /// <summary>
    /// The core parser that resolves an <see cref="AuthorizationHeader" /> from a set of <paramref name="credentials" />.
    /// </summary>
    /// <param name="credentials">The credentials used in authentication.</param>
    /// <returns>An <see cref="AuthorizationHeader" /> equivalent of <paramref name="credentials" />.</returns>
    protected override AuthorizationHeader ParseCore(IReadOnlyDictionary<string, string> credentials)
    {
        string clientId = null, credentialScope = null, signedHeaders = null, signature = null;
        foreach (var kvp in credentials)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key == CredentialComponent)
            {
                var cs = value.Split(ForwardSlashSeparator, 2);
                clientId = cs[0];
                credentialScope = cs[1];
            }

            if (key == SignedHeadersComponent)
            {
                signedHeaders = value;
            }

            if (key == SignatureComponent)
            {
                signature = value;
            }
        }

        if (!string.IsNullOrWhiteSpace(clientId) &&
            credentialScope != null &&
            !string.IsNullOrEmpty(signedHeaders) &&
            !string.IsNullOrWhiteSpace(signature))
        {
            return new HmacAuthorizationHeader(clientId, credentialScope, signedHeaders, signature, AuthenticationScheme);
        }

        return null;
    }
}
