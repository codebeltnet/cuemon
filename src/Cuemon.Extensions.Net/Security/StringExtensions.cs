using System;
using System.Globalization;
using System.Security;
using Cuemon.Net;
using Cuemon.Security.Cryptography;

namespace Cuemon.Extensions.Net.Security;
/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="uriString"/> to a signed and tampering protected <see cref="Uri"/>.
    /// </summary>
    /// <param name="uriString">The URI to protect from tampering.</param>
    /// <param name="secret">The secret key for the encryption.</param>
    /// <param name="start">The time, expressed as the Coordinated Universal Time (UTC), at which the signed URI becomes valid.</param>
    /// <param name="expiry">The time, expressed as the Coordinated Universal Time (UTC), at which the signed URI becomes invalid.</param>
    /// <param name="setup">The <see cref="SignedUriOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Uri"/> that is equivalent to <paramref name="uriString"/> but signed and protected from tampering.</returns>
    /// <remarks>If either of <paramref name="start"/> or <paramref name="expiry"/> has a <see cref="DateTime.Kind"/> different from <see cref="DateTimeKind.Utc"/>, they will be adjusted without changing the value of the <see cref="DateTime"/> itself.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="uriString"/> cannot be null - or -
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="uriString"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <seealso cref="ValidateSignedUri"/>
    public static Uri ToSignedUri(this string uriString, byte[] secret, DateTime? start = null, DateTime? expiry = null, Action<SignedUriOptions> setup = null)
    {
        Validator.ThrowIfNullOrWhitespace(uriString);
        Validator.ThrowIfNull(secret);

        var options = Patterns.Configure(setup);
        var qsc = uriString.ToQueryStringCollection();
        if (start.HasValue) { qsc.Add(options.StartFieldName, Decorator.Enclose(start.Value).ToUtcKind().ToString("s") + "Z"); }
        if (expiry.HasValue) { qsc.Add(options.ExpiryFieldName, Decorator.Enclose(expiry.Value).ToUtcKind().ToString("s") + "Z"); }
        uriString = FormattableString.Invariant($"{uriString.SkipQueryString()}{qsc.ToQueryString(options.UrlEncode)}");
        qsc.Add(options.SignatureFieldName, KeyedHashFactory.CreateHmacCrypto(secret, options.Algorithm).ComputeHash(options.CanonicalRepresentationBuilder(uriString)).ToUrlEncodedBase64String());

        return new Uri(FormattableString.Invariant($"{uriString.SkipQueryString()}{qsc.ToQueryString(options.UrlEncode)}"));
    }

    /// <summary>
    /// Reads and validates the specified <paramref name="signedUriString"/>.
    /// </summary>
    /// <param name="signedUriString">The signed URI that needs to be validated.</param>
    /// <param name="secret">The secret key for the encryption.</param>
    /// <param name="setup">The <see cref="SignedUriOptions"/> which may be configured.</param>
    /// <exception cref="SecurityException">
    /// <paramref name="signedUriString"/> has an invalid signature.
    /// </exception>
    /// <seealso cref="ToSignedUri"/>
    public static void ValidateSignedUri(this string signedUriString, byte[] secret, Action<SignedUriOptions> setup = null)
    {
        Validator.ThrowIfNullOrWhitespace(signedUriString);
        Validator.ThrowIfNull(secret);

        var message = "The specified signature is invalid.";
        var utcNow = DateTime.UtcNow;
        var options = Patterns.Configure(setup);
        var qsc = signedUriString.ToQueryStringCollection();
        var signature = qsc[options.SignatureFieldName];
        var signedStart = qsc[options.StartFieldName];
        var signedExpiry = qsc[options.ExpiryFieldName];
        if (!string.IsNullOrWhiteSpace(signedStart) && DateTime.Parse(signedStart, null, DateTimeStyles.RoundtripKind) > utcNow) { throw new SecurityException(message); }
        if (!string.IsNullOrWhiteSpace(signedExpiry) && DateTime.Parse(signedExpiry, null, DateTimeStyles.RoundtripKind) <= utcNow) { throw new SecurityException(message); }
        if (string.IsNullOrWhiteSpace(signature)) { throw new SecurityException(message); }
        qsc.Remove(options.SignatureFieldName);

        var computedSignature = KeyedHashFactory.CreateHmacCrypto(secret, options.Algorithm).ComputeHash(options.CanonicalRepresentationBuilder(FormattableString.Invariant($"{signedUriString.SkipQueryString()}{qsc.ToQueryString()}"))).ToUrlEncodedBase64String();
        if (!signature.Equals(computedSignature, StringComparison.Ordinal)) { throw new SecurityException(message); }
    }

    private static QueryStringCollection ToQueryStringCollection(this string uriString)
    {
        Validator.ThrowIfNull(uriString);
        return new QueryStringCollection(uriString.TakeQueryString());
    }

    private static string TakeQueryString(this string uriString)
    {
        Validator.ThrowIfNull(uriString);
        var indexOfQueryString = uriString.IndexOf('?');
        return indexOfQueryString > 0 ? uriString.Substring(indexOfQueryString) : "";
    }

    private static string SkipQueryString(this string uriString)
    {
        Validator.ThrowIfNull(uriString);
        var indexOfQueryString = uriString.IndexOf('?');
        return uriString.Substring(0, indexOfQueryString);
    }
}
