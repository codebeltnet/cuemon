namespace Cuemon.Security.Cryptography;
/// <summary>
/// Specifies the size of the Advanced Encryption Standard (AES) symmetric algorithm.
/// </summary>
public enum AesSize
{
    /// <summary>
    /// The Advanced Encryption Standard (AES) symmetric algorithm with a 128 bit key.
    /// </summary>
    Aes128 = 0,
    /// <summary>
    /// The Advanced Encryption Standard (AES) symmetric algorithm with a 192 bit key.
    /// </summary>
    Aes192 = 1,
    /// <summary>
    /// The Advanced Encryption Standard (AES) symmetric algorithm with a 256 bit key.
    /// </summary>
    Aes256 = 2,
}
