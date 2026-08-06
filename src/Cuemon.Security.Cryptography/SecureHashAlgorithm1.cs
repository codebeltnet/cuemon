using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a SHA-1 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 160-bit hash values. This class cannot be inherited.
/// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
/// <seealso cref="Hash{TOptions}" />
public sealed class SecureHashAlgorithm1 : UnkeyedCryptoHash<SHA1>
{
    /// <summary>
    /// Produces a 160-bit hash value
    /// </summary>
    public const int BitSize = 160;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureHashAlgorithm1"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    public SecureHashAlgorithm1(Action<ConvertibleOptions> setup = null) : base(SHA1.Create, setup)
    {
    }
}
