namespace Cuemon.Security.Cryptography;
/// <summary>
/// Specifies the different implementations of a cryptographic hashing algorithm.
/// </summary>
public enum UnkeyedCryptoAlgorithm
{
    /// <summary>
    /// The Message Digest 5 (MD5) algorithm (128 bits).
    /// </summary>
    Md5 = -2,
    /// <summary>
    /// The Secure Hashing Algorithm (SHA1) algorithm (160 bits).
    /// </summary>
    Sha1 = -1,
    /// <summary>
    /// The Secure Hashing Algorithm (SHA256) algorithm (256 bits).
    /// </summary>
    Sha256 = 0,
    /// <summary>
    /// The Secure Hashing Algorithm (SHA384) algorithm (384 bits).
    /// </summary>
    Sha384 = 1,
    /// <summary>
    /// The Secure Hashing Algorithm (SHA512) algorithm (512 bits).
    /// </summary>
    Sha512 = 2,
    /// <summary>
    /// The Secure Hashing Algorithm (SHA512/256) algorithm (256 bits).
    /// </summary>
    Sha512Slash256 = 3
}
