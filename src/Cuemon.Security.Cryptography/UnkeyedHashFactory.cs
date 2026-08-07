using System;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Hash"/> instances based on <see cref="UnkeyedCryptoHash{TAlgorithm}"/>.
/// </summary>
public static class UnkeyedHashFactory
{
    /// <summary>
    /// Creates an instance of a cryptographic implementation that derives from <see cref="UnkeyedCryptoHash{TAlgorithm}"/> with the specified <paramref name="algorithm"/>. Default is <see cref="SecureHashAlgorithm256"/>.
    /// </summary>
    /// <param name="algorithm">The <see cref="UnkeyedCryptoAlgorithm"/> that defines the cryptographic implementation. Default is <see cref="UnkeyedCryptoAlgorithm.Sha256"/>.</param>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of the by parameter specified <paramref name="algorithm"/>.</returns>
    public static Hash CreateCrypto(UnkeyedCryptoAlgorithm algorithm = default, Action<ConvertibleOptions> setup = null)
    {
        switch (algorithm)
        {
            case UnkeyedCryptoAlgorithm.Md5:
                return CreateCryptoMd5(setup);
            case UnkeyedCryptoAlgorithm.Sha1:
                return CreateCryptoSha1(setup);
            case UnkeyedCryptoAlgorithm.Sha384:
                return CreateCryptoSha384(setup);
            case UnkeyedCryptoAlgorithm.Sha512:
                return CreateCryptoSha512(setup);
            case UnkeyedCryptoAlgorithm.Sha512Slash256:
                return CreateCryptoSha512Slash256(setup);
            default:
                return CreateCryptoSha256(setup);
        }
    }

    /// <summary>
    /// Creates an instance of <see cref="SecureHashAlgorithm512256"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="SecureHashAlgorithm512256"/>.</returns>
    public static Hash CreateCryptoSha512Slash256(Action<ConvertibleOptions> setup = null)
    {
        return new SecureHashAlgorithm512256(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="SecureHashAlgorithm512"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="SecureHashAlgorithm512"/>.</returns>
    public static Hash CreateCryptoSha512(Action<ConvertibleOptions> setup = null)
    {
        return new SecureHashAlgorithm512(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="SecureHashAlgorithm384"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="SecureHashAlgorithm384"/>.</returns>
    public static Hash CreateCryptoSha384(Action<ConvertibleOptions> setup = null)
    {
        return new SecureHashAlgorithm384(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="SecureHashAlgorithm256"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="SecureHashAlgorithm256"/>.</returns>
    public static Hash CreateCryptoSha256(Action<ConvertibleOptions> setup = null)
    {
        return new SecureHashAlgorithm256(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="SecureHashAlgorithm1"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="SecureHashAlgorithm1"/>.</returns>
    public static Hash CreateCryptoSha1(Action<ConvertibleOptions> setup = null)
    {
        return new SecureHashAlgorithm1(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="MessageDigest5"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="MessageDigest5"/>.</returns>
    public static Hash CreateCryptoMd5(Action<ConvertibleOptions> setup = null)
    {
        return new MessageDigest5(setup);
    }
}
