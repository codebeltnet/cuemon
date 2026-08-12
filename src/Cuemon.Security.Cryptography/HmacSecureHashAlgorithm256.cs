using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a Hash-based Message Authentication Code (HMAC) by using the <see cref="SHA256"/> hash function. This class cannot be inherited.
/// Implements the <see cref="KeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="Hash{TOptions}" />
public sealed class HmacSecureHashAlgorithm256 : KeyedCryptoHash<HMACSHA256>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HmacSecureHashAlgorithm256"/> class.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm256"/> encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-256) to derive a 64-byte key. If it is less than 64 bytes long, it is padded to 64 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which need to be configured.</param>
    public HmacSecureHashAlgorithm256(byte[] secret, Action<ConvertibleOptions> setup) : base(secret, setup)
    {
    }
}
