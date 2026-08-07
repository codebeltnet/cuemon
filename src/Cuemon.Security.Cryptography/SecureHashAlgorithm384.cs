using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a SHA-384 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 384-bit hash values. This class cannot be inherited.
/// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
/// <seealso cref="Hash{TOptions}" />
public sealed class SecureHashAlgorithm384 : UnkeyedCryptoHash<SHA384>
{
    /// <summary>
    /// Produces a 384-bit hash value
    /// </summary>
    public const int BitSize = 384;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureHashAlgorithm384"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    public SecureHashAlgorithm384(Action<ConvertibleOptions> setup = null) : base(SHA384.Create, setup)
    {
    }
}
