namespace Cuemon.Security.Cryptography;
/// <summary>
/// Specifies the different implementations for generating hash-based message authentication code values.
/// </summary>
public enum KeyedCryptoAlgorithm
{
    /// <summary>
    /// The Hash-based Message Authentication Code using (MD5) algorithm (128 bits).
    /// </summary>
    HmacMd5 = -2,
    /// <summary>
    /// The Hash-based Message Authentication Code using (SHA1) algorithm (160 bits).
    /// </summary>
    HmacSha1 = -1,
    /// <summary>
    /// The Hash-based Message Authentication Code using (SHA256) algorithm (256 bits).
    /// </summary>
    HmacSha256 = 0,
    /// <summary>
    /// The Hash-based Message Authentication Code using (SHA384) algorithm (384 bits).
    /// </summary>
    HmacSha384 = 1,
    /// <summary>
    /// The Hash-based Message Authentication Code using (SHA512) algorithm (512 bits).
    /// </summary>
    HmacSha512 = 2
}
