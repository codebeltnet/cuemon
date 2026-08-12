using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a Hash-based Message Authentication Code (HMAC) by using the <see cref="MD5"/> hash function. This class cannot be inherited.
/// Implements the <see cref="KeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="Hash{TOptions}" />
public sealed class HmacMessageDigest5 : KeyedCryptoHash<HMACMD5>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HmacMessageDigest5"/> class.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacMessageDigest5"/> encryption. The key can be any length, but if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which need to be configured.</param>
    public HmacMessageDigest5(byte[] secret, Action<ConvertibleOptions> setup) : base(secret, setup)
    {
    }
}
