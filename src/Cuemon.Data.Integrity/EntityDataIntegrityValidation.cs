namespace Cuemon.Data.Integrity;
/// <summary>
/// Specifies the validation strength of a data checksum.
/// </summary>
public enum EntityDataIntegrityValidation
{
    /// <summary>
    /// Indicates that no checksum strength was specified.
    /// </summary>
    Unspecified = 0,
    /// <summary>
    /// Indicates that a weak, semantic equivalent checksum was specified.
    /// </summary>
    Weak,
    /// <summary>
    /// Indicates that a strong, byte-for-byte checksum was specified.
    /// </summary>
    Strong
}
