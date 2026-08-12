using System;
using Cuemon.Security;
using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Hash"/> instances based on <see cref="UnkeyedCryptoHash{TAlgorithm}"/>.
/// </summary>
public static class DigestHashFactory
{
    /// <summary>
    /// Creates an instance of a cryptographic implementation that derives from <see cref="UnkeyedCryptoHash{TAlgorithm}"/> with the specified <paramref name="algorithm"/>.
    /// </summary>
    /// <param name="algorithm">The <see cref="DigestCryptoAlgorithm"/> that defines the cryptographic implementation. Default is <see cref="DigestCryptoAlgorithm.Sha256"/>.</param>
    /// <returns>A <see cref="Hash"/> implementation of the by parameter specified <paramref name="algorithm"/>.</returns>
    public static Hash CreateCrypto(DigestCryptoAlgorithm algorithm = default)
    {
        switch (algorithm)
        {
            case DigestCryptoAlgorithm.Md5:
            case DigestCryptoAlgorithm.Md5Session:
                return UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Md5);
            case DigestCryptoAlgorithm.Sha256:
            case DigestCryptoAlgorithm.Sha256Session:
                return UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Sha256);
            case DigestCryptoAlgorithm.Sha512Slash256:
            case DigestCryptoAlgorithm.Sha512Slash256Session:
                return UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Sha512Slash256);
            default:
                throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, $"The specified {nameof(algorithm)} is not supported.");
        }
    }
}
