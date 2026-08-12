using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Provides a way to fluently represent a HTTP Digest Access Authentication header.
/// </summary>
public class DigestAuthorizationHeaderBuilder : AuthorizationHeaderBuilder<DigestAuthorizationHeader, DigestAuthorizationHeaderBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthorizationHeaderBuilder"/> class.
    /// </summary>
    public DigestAuthorizationHeaderBuilder() : this(DigestCryptoAlgorithm.Sha256)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthorizationHeaderBuilder"/> class.
    /// </summary>
    /// <param name="algorithm">The algorithm to use when computing HA1, HA2 and/or RESPONSE value(s).</param>
    public DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm algorithm) : base(DigestAuthorizationHeader.Scheme)
    {
        DigestAlgorithm = algorithm;
        MapRelation(nameof(AddResponse), DigestFields.Response);
        MapRelation(nameof(AddRealm), DigestFields.Realm);
        MapRelation(nameof(AddUserName), DigestFields.UserName);
        MapRelation(nameof(AddUri), DigestFields.DigestUri);
        MapRelation(nameof(AddNc), DigestFields.NonceCount);
        MapRelation(nameof(AddCnonce), DigestFields.ClientNonce);
        MapRelation(nameof(AddQopAuthentication), DigestFields.QualityOfProtection);
        MapRelation(nameof(AddQopAuthenticationIntegrity), DigestFields.QualityOfProtection);
        MapRelation(nameof(ComputeHash1), DigestFields.UserName, DigestFields.Realm);
        MapRelation(nameof(ComputeHash2), DigestFields.QualityOfProtection, DigestFields.DigestUri);
        MapRelation(nameof(ComputeResponse), DigestFields.Nonce, DigestFields.NonceCount, DigestFields.ClientNonce, DigestFields.QualityOfProtection);
    }

    /// <summary>
    /// Gets the algorithm of the HTTP Digest Access Authentication.
    /// </summary>
    /// <value>The algorithm of the HTTP Digest Access Authentication.</value>
    public DigestCryptoAlgorithm DigestAlgorithm { get; private set; }

    /// <summary>
    /// Associates the <see cref="DigestFields.Realm"/> field with the specified <paramref name="realm"/>.
    /// </summary>
    /// <param name="realm">The realm that defines the remote resource.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddRealm(string realm)
    {
        return AddOrUpdate(DigestFields.Realm, realm);
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.UserName"/> field with the specified <paramref name="username"/>.
    /// </summary>
    /// <param name="username">The username to use in the authentication process.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddUserName(string username)
    {
        Validator.ThrowIfNullOrWhitespace(username);
        return AddOrUpdate(DigestFields.UserName, username);
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.DigestUri"/> field with the specified <paramref name="digestUri"/>.
    /// </summary>
    /// <param name="digestUri">The effective request URI to use in the authentication process.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddUri(string digestUri)
    {
        Validator.ThrowIfNullOrWhitespace(digestUri);
        return AddOrUpdate(DigestFields.DigestUri, digestUri);
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.NonceCount"/> field with the specified <paramref name="nonceCount"/>.
    /// </summary>
    /// <param name="nonceCount">The count of the number of requests to use in the authentication process.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddNc(int nonceCount)
    {
        Validator.ThrowIfLowerThan(nonceCount, 0, nameof(nonceCount));
        return AddOrUpdate(DigestFields.NonceCount, nonceCount.ToString("x8", CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.ClientNonce"/> field with the specified <paramref name="clientNonce"/>.
    /// </summary>
    /// <param name="clientNonce">The cryptographic client nonce to use in the authentication process.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddCnonce(string clientNonce = null)
    {
        clientNonce ??= Generate.RandomString(32);
        return AddOrUpdate(DigestFields.ClientNonce, clientNonce);
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.QualityOfProtection"/> field with "auth".
    /// </summary>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddQopAuthentication()
    {
        return AddOrUpdate(DigestFields.QualityOfProtection, "auth");
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.QualityOfProtection"/> field with "auth-int".
    /// </summary>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddQopAuthenticationIntegrity()
    {
        return AddOrUpdate(DigestFields.QualityOfProtection, "auth-int");
    }

    /// <summary>
    /// Associates any Digest fields found in the HTTP WWW-Authenticate header from the specified <paramref name="headers"/>.
    /// </summary>
    /// <param name="headers">An instance of <see cref="HttpResponseHeaders"/>.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddFromWwwAuthenticateHeader(HttpResponseHeaders headers)
    {
        Validator.ThrowIfNull(headers);
        return AddFromWwwAuthenticateHeader(headers.WwwAuthenticate.ToString());
    }

    /// <summary>
    /// Associates any Digest fields found in the HTTP WWW-Authenticate header from the specified <paramref name="headers"/>.
    /// </summary>
    /// <param name="headers">An implementation of <see cref="IHeaderDictionary"/>.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddFromWwwAuthenticateHeader(IHeaderDictionary headers)
    {
        Validator.ThrowIfNull(headers);
        return AddFromWwwAuthenticateHeader(headers[HeaderNames.WWWAuthenticate]);
    }

    private DigestAuthorizationHeaderBuilder AddFromWwwAuthenticateHeader(string wwwAuthenticateHeader)
    {
        Validator.ThrowIfNull(wwwAuthenticateHeader);
        Validator.ThrowIfFalse(() => wwwAuthenticateHeader.StartsWith(AuthenticationScheme, StringComparison.OrdinalIgnoreCase), nameof(wwwAuthenticateHeader), $"Header did not start with {AuthenticationScheme}.");
        var headerWithoutScheme = wwwAuthenticateHeader.Remove(0, AuthenticationScheme.Length + 1);
        var fields = DelimitedString.Split(headerWithoutScheme);
        foreach (var field in fields)
        {
            var kvp = DelimitedString.Split(field, o => o.Delimiter = "=");
            var key = kvp[0].Trim();
            var value = kvp[1].Trim('"');
            if (key == DigestFields.QualityOfProtection) { continue; }
            AddOrUpdate(key, value);
        }
        return this;
    }

    /// <summary>
    /// Associates any Digest fields found in the HTTP Authorization header from the specified <paramref name="header"/>.
    /// </summary>
    /// <param name="header">An instance of <see cref="DigestAuthorizationHeader"/>.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddFromDigestAuthorizationHeader(DigestAuthorizationHeader header)
    {
        Validator.ThrowIfNull(header);
        DigestAlgorithm = ParseAlgorithm(header.Algorithm);
        AddUserName(header.UserName);
        AddRealm(header.Realm);
        AddUri(header.Uri);
        AddNc(Convert.ToInt32(header.NC, 16));
        AddCnonce(header.CNonce);
        AddOrUpdate(DigestFields.Nonce, header.Nonce);
        Condition.FlipFlop(header.Qop.Equals("auth-int", StringComparison.OrdinalIgnoreCase), () => AddQopAuthenticationIntegrity(), () => AddQopAuthentication());
        return this;
    }

    private static DigestCryptoAlgorithm ParseAlgorithm(string algorithm)
    {
        if (algorithm.Equals("MD5", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Md5; }
        if (algorithm.Equals("SHA-256", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Sha256; }
        if (algorithm.Equals("SHA-512-256", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Sha512Slash256; }
        if (algorithm.Equals("MD5-sess", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Md5Session; }
        if (algorithm.Equals("SHA-256-sess", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Sha256Session; }
        if (algorithm.Equals("SHA-512-256-sess", StringComparison.OrdinalIgnoreCase)) { return DigestCryptoAlgorithm.Sha512Slash256Session; }
        throw new NotSupportedException($"The algorithm '{algorithm}' is not supported.");
    }

    /// <summary>
    /// Associates the <see cref="DigestFields.Response"/> field with the computed values of <see cref="ComputeHash1"/>, <see cref="ComputeHash2"/> and <see cref="ComputeResponse"/>.
    /// </summary>
    /// <param name="password">The password to include in the HA1 computed value.</param>
    /// <param name="method">The HTTP method to include in the HA2 computed value.</param>
    /// <param name="entityBody">The entity body to apply in the signature when qop is set to auth-int.</param>
    /// <returns>An <see cref="DigestAuthorizationHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
    public DigestAuthorizationHeaderBuilder AddResponse(string password, string method, string entityBody = null)
    {
        Validator.ThrowIfNullOrWhitespace(password);
        Validator.ThrowIfNullOrWhitespace(method);
        ValidateData(DigestFields.UserName, DigestFields.Realm, DigestFields.QualityOfProtection, DigestFields.DigestUri, DigestFields.Nonce, DigestFields.NonceCount, DigestFields.ClientNonce);
        return AddOrUpdate(DigestFields.Response, ComputeResponse(ComputeHash1(password), ComputeHash2(method, entityBody)));
    }

    /// <summary>
    /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA1.
    /// </summary>
    /// <param name="password">The password to include in the HA1 computed value.</param>
    /// <returns>A <see cref="string"/> in the format of H(<see cref="DigestFields.UserName"/>:<see cref="DigestFields.Realm"/>:<paramref name="password"/>). H is determined by <see cref="DigestAlgorithm"/>.</returns>
    public virtual string ComputeHash1(string password)
    {
        Validator.ThrowIfNullOrWhitespace(password);
        ValidateData(DigestFields.UserName, DigestFields.Realm);
        var crypto = DigestHashFactory.CreateCrypto(DigestAlgorithm);

        var ha1 = crypto.ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", Data[DigestFields.UserName], Data[DigestFields.Realm], password), o =>
        {
            o.Encoding = Encoding.UTF8;
        }).ToHexadecimalString();

        switch (DigestAlgorithm)
        {
            case DigestCryptoAlgorithm.Md5Session:
            case DigestCryptoAlgorithm.Sha256Session:
            case DigestCryptoAlgorithm.Sha512Slash256Session:
                ha1 = crypto.ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", ha1, Data[DigestFields.Nonce], Data[DigestFields.ClientNonce]), o =>
                {
                    o.Encoding = Encoding.UTF8;
                }).ToHexadecimalString();
                break;
        }

        return ha1;
    }

    /// <summary>
    /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA2.
    /// </summary>
    /// <param name="method">The HTTP method to include in the HA2 computed value.</param>
    /// <param name="entityBody">The entity body to apply in the signature when qop is set to auth-int.</param>
    /// <returns>A <see cref="string"/> in the format of H(<paramref name="method"/>:<see cref="DigestFields.DigestUri"/>) OR H(<paramref name="method"/>:<see cref="DigestFields.DigestUri"/>:H(<paramref name="entityBody"/>)). H is determined by <see cref="DigestAlgorithm"/>.</returns>
    public virtual string ComputeHash2(string method, string entityBody = null)
    {
        Validator.ThrowIfNullOrWhitespace(method);
        method = method.ToUpperInvariant();
        ValidateData(DigestFields.QualityOfProtection, DigestFields.DigestUri);
        var qop = Data[DigestFields.QualityOfProtection];
        var hasIntegrityProtection = qop.Equals("auth-int", StringComparison.OrdinalIgnoreCase);
        if (hasIntegrityProtection && entityBody == null) { throw new ArgumentNullException(nameof(entityBody), "The entity body cannot be null when qop is set to auth-int."); }

        var hashFields = !hasIntegrityProtection
            ? string.Create(CultureInfo.InvariantCulture, $"{method}:{Data[DigestFields.DigestUri]}")
            : string.Create(CultureInfo.InvariantCulture, $"{method}:{Data[DigestFields.DigestUri]}:{DigestHashFactory.CreateCrypto(DigestAlgorithm).ComputeHash(entityBody, o => o.Encoding = Encoding.UTF8).ToHexadecimalString()}");
        return DigestHashFactory.CreateCrypto(DigestAlgorithm).ComputeHash(hashFields, o =>
        {
            o.Encoding = Encoding.UTF8;
        }).ToHexadecimalString();
    }

    /// <summary>
    /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication RESPONSE.
    /// </summary>
    /// <param name="hash1">The HA1 to include in the RESPONSE computed value.</param>
    /// <param name="hash2">The HA2 to include in the RESPONSE computed value.</param>
    /// <returns>A <see cref="string"/> in the format of H(<paramref name="hash1"/>:<see cref="DigestFields.Nonce"/>:<see cref="DigestFields.NonceCount"/>:<see cref="DigestFields.ClientNonce"/>:<see cref="DigestFields.QualityOfProtection"/>:<paramref name="hash2"/>). H is determined by <see cref="DigestAlgorithm"/>.</returns>
    public virtual string ComputeResponse(string hash1, string hash2)
    {
        Validator.ThrowIfNullOrWhitespace(hash1);
        Validator.ThrowIfNullOrWhitespace(hash2);
        ValidateData(DigestFields.Nonce, DigestFields.NonceCount, DigestFields.ClientNonce, DigestFields.QualityOfProtection);
        return DigestHashFactory.CreateCrypto(DigestAlgorithm).ComputeHash(string.Create(CultureInfo.InvariantCulture, $"{hash1}:{Data[DigestFields.Nonce]}:{Data[DigestFields.NonceCount]}:{Data[DigestFields.ClientNonce]}:{Data[DigestFields.QualityOfProtection]}:{hash2}"), o =>
        {
            o.Encoding = Encoding.UTF8;
        }).ToHexadecimalString();
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override DigestAuthorizationHeader Build()
    {
        ValidateData(DigestFields.Realm, DigestFields.Nonce, DigestFields.UserName, DigestFields.QualityOfProtection, DigestFields.DigestUri, DigestFields.NonceCount, DigestFields.ClientNonce, DigestFields.QualityOfProtection, DigestFields.Response);
        return new DigestAuthorizationHeader(Data[DigestFields.Realm],
            Data[DigestFields.Nonce],
            Data[DigestFields.Opaque],
            Data[DigestFields.Algorithm],
            Data[DigestFields.UserName],
            Data[DigestFields.DigestUri],
            Data[DigestFields.NonceCount],
            Data[DigestFields.ClientNonce],
            Data[DigestFields.QualityOfProtection],
            Data[DigestFields.Response]);
    }
}
