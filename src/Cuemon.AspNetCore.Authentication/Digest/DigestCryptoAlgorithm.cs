namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Specifies the cryptographic algorithms used in Digest authentication.
/// </summary>
public enum DigestCryptoAlgorithm
{
    /// <summary>
    /// The Message Digest 5 (MD5) algorithm (128 bits).
    /// </summary>
    Md5 = -2,

    /// <summary>
    /// The Message Digest 5 (MD5) algorithm (128 bits) session variant.
    /// </summary>
    Md5Session = -1,

    /// <summary>
    /// The Secure Hashing Algorithm (SHA256) algorithm (256 bits).
    /// </summary>
    Sha256 = 0,

    /// <summary>
    /// The Secure Hashing Algorithm (SHA256) algorithm (256 bits) session variant.
    /// </summary>
    Sha256Session = 1,

    /// <summary>
    /// The Secure Hashing Algorithm (SHA512/256) algorithm (256 bits).
    /// </summary>
    Sha512Slash256 = 2,

    /// <summary>
    /// The Secure Hashing Algorithm (SHA512/256) algorithm (256 bits) session variant.
    /// </summary>
    Sha512Slash256Session = 3
}
