using System;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a SHA-512-256 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 512-bit hash values. This class cannot be inherited.
/// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
/// <seealso cref="Hash{TOptions}" />
public sealed class SecureHashAlgorithm512256 : UnkeyedCryptoHash<SHA512256>
{
    /// <summary>
    /// Produces a 256-bit hash value
    /// </summary>
    public const int BitSize = 256;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureHashAlgorithm512256"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    public SecureHashAlgorithm512256(Action<ConvertibleOptions> setup) : base(() => new SHA512256(), setup)
    {
    }
}
