using System;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Hash"/> instances based on <see cref="KeyedCryptoHash{TAlgorithm}"/>.
/// </summary>
public static class KeyedHashFactory
{
    /// <summary>
    /// Creates an instance of a HMAC cryptographic implementation that derives from <see cref="KeyedCryptoHash{TAlgorithm}"/> with the specified <paramref name="algorithm"/>. Default is <see cref="HmacSecureHashAlgorithm256"/>.
    /// </summary>
    /// <param name="secret">The secret key for the encryption.</param>
    /// <param name="algorithm">The <see cref="KeyedCryptoAlgorithm"/> that defines the HMAC cryptographic implementation. Default is <see cref="KeyedCryptoAlgorithm.HmacSha256"/>.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of the by parameter specified <paramref name="algorithm"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCrypto(byte[] secret, KeyedCryptoAlgorithm algorithm = default, Action<ConvertibleOptions> setup = null)
    {
        switch (algorithm)
        {
            case KeyedCryptoAlgorithm.HmacMd5:
                return CreateHmacCryptoMd5(secret, setup);
            case KeyedCryptoAlgorithm.HmacSha1:
                return CreateHmacCryptoSha1(secret, setup);
            case KeyedCryptoAlgorithm.HmacSha384:
                return CreateHmacCryptoSha384(secret, setup);
            case KeyedCryptoAlgorithm.HmacSha512:
                return CreateHmacCryptoSha512(secret, setup);
            default:
                return CreateHmacCryptoSha256(secret, setup);
        }
    }

    /// <summary>
    /// Creates an instance of <see cref="HmacSecureHashAlgorithm512"/>.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm512"/> encryption. The key can be any length. However, the recommended size is 128 bytes. If the key is more than 128 bytes long, it is hashed (using SHA-384) to derive a 128-byte key. If it is less than 128 bytes long, it is padded to 128 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="HmacSecureHashAlgorithm512"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCryptoSha512(byte[] secret, Action<ConvertibleOptions> setup = null)
    {
        Validator.ThrowIfNull(secret);
        return new HmacSecureHashAlgorithm512(secret, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="HmacSecureHashAlgorithm384"/>.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm384"/> encryption. The key can be any length. However, the recommended size is 128 bytes. If the key is more than 128 bytes long, it is hashed (using SHA-384) to derive a 128-byte key. If it is less than 128 bytes long, it is padded to 128 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="HmacSecureHashAlgorithm384"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCryptoSha384(byte[] secret, Action<ConvertibleOptions> setup = null)
    {
        Validator.ThrowIfNull(secret);
        return new HmacSecureHashAlgorithm384(secret, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="HmacSecureHashAlgorithm256"/>.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm256"/> encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-256) to derive a 64-byte key. If it is less than 64 bytes long, it is padded to 64 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="HmacSecureHashAlgorithm256"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCryptoSha256(byte[] secret, Action<ConvertibleOptions> setup = null)
    {
        Validator.ThrowIfNull(secret);
        return new HmacSecureHashAlgorithm256(secret, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="HmacSecureHashAlgorithm1"/>.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacSecureHashAlgorithm1"/> encryption. The key can be any length, but if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="HmacSecureHashAlgorithm1"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCryptoSha1(byte[] secret, Action<ConvertibleOptions> setup = null)
    {
        Validator.ThrowIfNull(secret);
        return new HmacSecureHashAlgorithm1(secret, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="HmacMessageDigest5"/>.
    /// </summary>
    /// <param name="secret">The secret key for <see cref="HmacMessageDigest5"/> encryption. The key can be any length, but if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="HmacMessageDigest5"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="secret"/> cannot be null.
    /// </exception>
    public static Hash CreateHmacCryptoMd5(byte[] secret, Action<ConvertibleOptions> setup = null)
    {
        Validator.ThrowIfNull(secret);
        return new HmacMessageDigest5(secret, setup);
    }
}
