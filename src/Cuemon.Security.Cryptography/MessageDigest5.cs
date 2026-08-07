using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Provides a MD5 implementation of the MD (Message Digest) cryptographic hashing algorithm for 128-bit hash values. This class cannot be inherited.
/// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
/// </summary>
/// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
/// <seealso cref="Hash{TOptions}" />
public sealed class MessageDigest5 : UnkeyedCryptoHash<MD5>
{
    /// <summary>
    /// Produces a 128-bit hash value
    /// </summary>
    public const int BitSize = 128;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDigest5"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    public MessageDigest5(Action<ConvertibleOptions> setup = null) : base(MD5.Create, setup)
    {
    }
}
