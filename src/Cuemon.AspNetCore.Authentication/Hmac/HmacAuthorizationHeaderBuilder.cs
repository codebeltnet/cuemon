using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Cuemon.Collections.Generic;
using Cuemon.Net;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Hmac
{
    /// <summary>
    /// Provides a way to fluently represent a HTTP HMAC Authentication header.
    /// </summary>
    public class HmacAuthorizationHeaderBuilder : HmacAuthorizationHeaderBuilder<HmacAuthorizationHeaderBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthorizationHeaderBuilder"/> class.
        /// </summary>
        /// <param name="authenticationScheme">The name of the authentication scheme. Default is HMAC.</param>
        /// <param name="hmacAlgorithm">The algorithm to use when computing the final signature of the HMAC Authentication header.</param>
        /// <param name="algorithm">The algorithm to use when computing in-between signatures as part of the final signing of the HMAC Authentication header.</param>
        public HmacAuthorizationHeaderBuilder(string authenticationScheme = HmacFields.Scheme, KeyedCryptoAlgorithm hmacAlgorithm = KeyedCryptoAlgorithm.HmacSha256, UnkeyedCryptoAlgorithm algorithm = UnkeyedCryptoAlgorithm.Sha256) : base(authenticationScheme)
        {
            HmacAlgorithm = hmacAlgorithm;
            Algorithm = algorithm;
            MapRelation(nameof(AddCredentialScope), HmacFields.CredentialScope);
            MapRelation(nameof(AddFromRequest), HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload, HmacFields.ServerDateTime);
        }

        /// <summary>
        /// Gets the non-keyed algorithm of the HTTP HMAC Authentication.
        /// </summary>
        /// <value>The non-keyed algorithm of the HTTP HMAC Authentication.</value>
        public UnkeyedCryptoAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the keyed algorithm of the HTTP HMAC Authentication.
        /// </summary>
        /// <value>The keyed algorithm of the HTTP HMAC Authentication.</value>
        public KeyedCryptoAlgorithm HmacAlgorithm { get; }

        /// <summary>
        /// Adds the necessary fields that is part of an HTTP request.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequestMessage" /> object.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public HmacAuthorizationHeaderBuilder AddFromRequest(HttpRequestMessage request)
        {
            var queryNvc = HttpUtility.ParseQueryString(request.RequestUri?.Query ?? "");
            return AddOrUpdate(HmacFields.HttpMethod, request.Method.Method)
                .AddOrUpdate(HmacFields.UriPath, request.RequestUri!.AbsolutePath)
                .AddOrUpdate(HmacFields.UriQuery, string.Concat(queryNvc.Cast<string>().Select((s, i) => new KeyValuePair<string, StringValues>(s, queryNvc[i]?.Split(','))).OrderBy(pair => pair.Key).Select(pair => $"{Decorator.Enclose(pair.Key).UrlEncode()}={Decorator.Enclose(pair.Value.ToString()).UrlEncode()}")))
                .AddOrUpdate(HmacFields.HttpHeaders, !request.Headers.Any() ? null : string.Concat(request.Headers.OrderBy(pair => pair.Key).Select(pair => $"{pair.Key.ToLowerInvariant()}:{DelimitedString.Create(pair.Value, o => o.StringConverter = s => $"{s.Trim()}{Alphanumeric.Linefeed}")}")))
                .AddOrUpdate(HmacFields.Payload, UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(request.Content?.ReadAsStream() ?? new MemoryStream()).ToHexadecimalString())
                .AddOrUpdate(HmacFields.ServerDateTime, request.Headers.Date?.UtcDateTime.ToString("O", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds the necessary fields that is part of an HTTP request.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest" /> object.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public override HmacAuthorizationHeaderBuilder AddFromRequest(HttpRequest request)
        {
            Validator.ThrowIfNull(request);
            return AddOrUpdate(HmacFields.HttpMethod, request.Method)
                .AddOrUpdate(HmacFields.UriPath, request.Path.ToUriComponent())
                .AddOrUpdate(HmacFields.UriQuery, string.Concat(request.Query.OrderBy(pair => pair.Key).Select(pair => $"{Decorator.Enclose(pair.Key).UrlEncode()}={Decorator.Enclose(pair.Value.ToString()).UrlEncode()}")))
                .AddOrUpdate(HmacFields.HttpHeaders, request.Headers.Count == 0 ? null : string.Concat(request.Headers.OrderBy(pair => pair.Key).Select(pair => $"{pair.Key.ToLowerInvariant()}:{DelimitedString.Create(pair.Value, o => o.StringConverter = s => $"{s.Trim()}{Alphanumeric.Linefeed}")}")))
                .AddOrUpdate(HmacFields.Payload, UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(request.Body).ToHexadecimalString())
                .AddOrUpdate(HmacFields.ServerDateTime, DateTime.Parse(request.Headers[HeaderNames.Date].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToString("O"));
        }

        /// <summary>
        /// Adds the credential scope that defines the remote resource.
        /// </summary>
        /// <param name="credentialScope">The credential scope that defines the remote resource.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public HmacAuthorizationHeaderBuilder AddCredentialScope(string credentialScope)
        {
            return AddOrUpdate(HmacFields.CredentialScope, credentialScope);
        }

        /// <summary>
        /// Converts the request to a standardized (canonical) format and computes a message digest.
        /// </summary>
        /// <returns>A <see cref="System.String" /> representation, in hexadecimal, of the computed canonical request.</returns>
        public override string ComputeCanonicalRequest()
        {
            ValidateData(HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            EnsureSignedHeaders(out var signedHeaders);
            var signedHeadersLookup = signedHeaders.Split(';').ToList();
            var headersToSign = DelimitedString.Create(Data[HmacFields.HttpHeaders].Split(Alphanumeric.Linefeed.ToCharArray()).Where(header =>
            {
                var kvp = header.Split(':');
                return signedHeadersLookup.Contains(kvp[0]);
            }), o => o.Delimiter = Alphanumeric.Linefeed) + Alphanumeric.Linefeed;

            var canonicalRequest = new StringBuilder(Data[HmacFields.HttpMethod])
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.UriPath])
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.UriQuery])
                .Append(Alphanumeric.LinefeedChar)
                .Append(headersToSign)
                .Append(Alphanumeric.LinefeedChar)
                .Append(signedHeaders)
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.Payload]).ToString();

            AddOrUpdate(HmacFields.CanonicalRequest, canonicalRequest);

            return UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(canonicalRequest).ToHexadecimalString();
        }

        /// <summary>
        /// Computes the signature of this instance using a series of hash-based message authentication codes (HMACs).
        /// </summary>
        /// <returns>A <see cref="System.String" /> representation, in hexadecimal, of the computed signature of this instance.</returns>
        public override string ComputeSignature()
        {
            ValidateData(HmacFields.ServerDateTime, HmacFields.ClientSecret);
            var secret = Decorator.Enclose(Data[HmacFields.ClientSecret]).ToByteArray();
            var stringToSign = string.Concat(Algorithm,
                Alphanumeric.Linefeed,
                Data[HmacFields.ServerDateTime],
                Alphanumeric.Linefeed,
                Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope),
                Alphanumeric.Linefeed,
                ComputeCanonicalRequest());
            var date = DateTime.Parse(Data[HmacFields.ServerDateTime], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var dateSecret = KeyedHashFactory.CreateHmacCrypto(secret, HmacAlgorithm).ComputeHash(date).GetBytes();

            AddOrUpdate(HmacFields.StringToSign, stringToSign);

            return KeyedHashFactory.CreateHmacCrypto(dateSecret, HmacAlgorithm).ComputeHash(stringToSign).ToHexadecimalString();
        }

        /// <summary>
        /// Builds an instance of <see cref="HmacAuthorizationHeader"/> that implements <see cref="AuthorizationHeader" />.
        /// </summary>
        /// <returns>An instance of <see cref="HmacAuthorizationHeader"/>.</returns>
        public override HmacAuthorizationHeader Build()
        {
            ValidateData(HmacFields.ClientId, HmacFields.ServerDateTime, HmacFields.ClientSecret, HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            EnsureSignedHeaders(out var signedHeaders);
            return new HmacAuthorizationHeader(Data[HmacFields.ClientId], Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope), signedHeaders, ComputeSignature(), AuthenticationScheme);
        }

        private void EnsureSignedHeaders(out string signedHeaders)
        {
            if (!Data.TryGetValue(HmacFields.SignedHeaders, out signedHeaders))
            {
                signedHeaders = "host;date";
                AddSignedHeaders(signedHeaders.Split(HmacFields.SignedHeadersDelimiter));
            }
        }
    }

    /// <summary>
    /// Represents the base class from which all builder implementations that represent a HTTP HMAC Authentication header should derive.
    /// </summary>
    public abstract class HmacAuthorizationHeaderBuilder<TAuthorizationHeaderBuilder> : AuthorizationHeaderBuilder<HmacAuthorizationHeader, TAuthorizationHeaderBuilder> where TAuthorizationHeaderBuilder : HmacAuthorizationHeaderBuilder<TAuthorizationHeaderBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthorizationHeaderBuilder{TAuthorizationHeaderBuilder}"/> class.
        /// </summary>
        /// <param name="authenticationScheme">The name of the authentication scheme.</param>
        protected HmacAuthorizationHeaderBuilder(string authenticationScheme) : base(authenticationScheme)
        {
            MapRelation(nameof(AddClientId), HmacFields.ClientId);
            MapRelation(nameof(AddClientSecret), HmacFields.ClientSecret);
        }

        /// <summary>
        /// Adds the client identifier that is the public key of the signing process.
        /// </summary>
        /// <param name="clientId">The client identifier that is the public key of the signing process.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public TAuthorizationHeaderBuilder AddClientId(string clientId)
        {
            return AddOrUpdate(HmacFields.ClientId, clientId);
        }

        /// <summary>
        /// Adds the client secret that is the private key of the signing process.
        /// </summary>
        /// <param name="clientSecret">The client secret that is the private key of the signing process.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public TAuthorizationHeaderBuilder AddClientSecret(string clientSecret)
        {
            return AddOrUpdate(HmacFields.ClientSecret, clientSecret);
        }

        /// <summary>
        /// Adds the necessary fields that is part of an HTTP request.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public abstract TAuthorizationHeaderBuilder AddFromRequest(HttpRequest request);

        /// <summary>
        /// Adds the headers that will be part of the signing process.
        /// </summary>
        /// <param name="signedHeaders">The headers that will be part of the signing process.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public virtual TAuthorizationHeaderBuilder AddSignedHeaders(params string[] signedHeaders)
        {
            if (signedHeaders == null) { return this as TAuthorizationHeaderBuilder; }
            return AddOrUpdate(HmacFields.SignedHeaders, DelimitedString.Create(signedHeaders.OrderBy(s => s), o =>
            {
                o.Delimiter = HmacFields.SignedHeadersDelimiter.ToString();
                o.StringConverter = s => s.ToLowerInvariant();
            }));
        }

        /// <summary>
        /// Converts the request to a standardized (canonical) format and computes a message digest.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the computed canonical request.</returns>
        public abstract string ComputeCanonicalRequest();

        /// <summary>
        /// Computes the signature of this instance using a series of hash-based message authentication codes (HMACs).
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the computed signature of this instance.</returns>
        public abstract string ComputeSignature();
    }
}
