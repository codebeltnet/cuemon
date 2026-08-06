using System;
using System.Security;

namespace Cuemon.Extensions.Net.Security;
/// <summary>
/// Extension methods for the <see cref="Uri"/> class.
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="uri"/> to a signed and tampering protected <see cref="Uri"/>.
    /// </summary>
    /// <param name="uri">The URI to protect from tampering.</param>
    /// <param name="secret">The secret key for the encryption.</param>
    /// <param name="signedStart">The time, expressed as the Coordinated Universal Time (UTC), at which the signed URI becomes valid.</param>
    /// <param name="signedExpiry">The time, expressed as the Coordinated Universal Time (UTC), at which the signed URI becomes invalid.</param>
    /// <param name="setup">The <see cref="SignedUriOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Uri"/> that is equivalent to <paramref name="uri"/> but signed and protected from tampering.</returns>
    public static Uri ToSignedUri(this Uri uri, byte[] secret, DateTime? signedStart = null, DateTime? signedExpiry = null, Action<SignedUriOptions> setup = null)
    {
        Validator.ThrowIfNull(uri);
        Validator.ThrowIfNull(secret);
        return uri.OriginalString.ToSignedUri(secret, signedStart, signedExpiry, setup);
    }

    /// <summary>
    /// Reads and validates the specified <paramref name="signedUri"/>.
    /// </summary>
    /// <param name="signedUri">The signed URI that needs to be validated.</param>
    /// <param name="secret">The secret key for the encryption.</param>
    /// <param name="setup">The <see cref="SignedUriOptions"/> which may be configured.</param>
    /// <exception cref="SecurityException">
    /// <paramref name="signedUri"/> did not have a signature specified - or -
    /// <paramref name="signedUri"/> has an invalid signature.
    /// </exception>
    /// <seealso cref="ToSignedUri"/>
    public static void ValidateSignedUri(this Uri signedUri, byte[] secret, Action<SignedUriOptions> setup = null)
    {
        Validator.ThrowIfNull(signedUri);
        Validator.ThrowIfNull(secret);
        signedUri.OriginalString.ValidateSignedUri(secret, setup);
    }
}
