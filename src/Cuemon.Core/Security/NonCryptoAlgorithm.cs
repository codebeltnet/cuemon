namespace Cuemon.Security;
/// <summary>
/// Specifies the different implementations of a non-cryptographic hashing algorithm.
/// </summary>
public enum NonCryptoAlgorithm
{
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (32 bits).
    /// </summary>
    Fnv32 = 0,
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (64 bits).
    /// </summary>
    Fnv64 = 1,
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (128 bits).
    /// </summary>
    Fnv128 = 2,
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (256 bits).
    /// </summary>
    Fnv256 = 3,
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (512 bits).
    /// </summary>
    Fnv512 = 4,
    /// <summary>
    /// The Fowler–Noll–Vo (FNV-1/FNV-1A) algorithm (1024 bits).
    /// </summary>
    Fnv1024 = 5
}
