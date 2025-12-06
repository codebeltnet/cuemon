using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Provides a SHA-512 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 512-bit hash values. This class cannot be inherited.
    /// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
    /// </summary>
    /// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
    /// <seealso cref="Hash{TOptions}" />
    public sealed class SecureHashAlgorithm512 : UnkeyedCryptoHash<SHA512>
    {
        /// <summary>
        /// Produces a 512-bit hash value
        /// </summary>
        public const int BitSize = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureHashAlgorithm512"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
        public SecureHashAlgorithm512(Action<ConvertibleOptions> setup = null) : base(SHA512.Create, setup)
        {
        }
    }
}
