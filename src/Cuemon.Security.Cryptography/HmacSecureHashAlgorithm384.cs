using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a Hash-based Message Authentication Code (HMAC) by using the <see cref="SHA384"/> hash function. This class cannot be inherited.
/// Implements the <see cref="KeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="Hash{TOptions}" />
public sealed class HmacSecureHashAlgorithm384 : KeyedCryptoHash<HMACSHA384>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HmacSecureHashAlgorithm384"/> class.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm384"/> encryption. The key can be any length. However, the recommended size is 128 bytes. If the key is more than 128 bytes long, it is hashed (using SHA-384) to derive a 128-byte key. If it is less than 128 bytes long, it is padded to 128 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which need to be configured.</param>
    public HmacSecureHashAlgorithm384(byte[] secret, Action<ConvertibleOptions> setup) : base(secret, setup)
    {
    }
}
