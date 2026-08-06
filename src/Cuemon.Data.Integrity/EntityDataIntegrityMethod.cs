namespace Cuemon.Data.Integrity;
/// <summary>
/// Specifies ways for a checksum of data to be computed.
/// </summary>
public enum EntityDataIntegrityMethod
{
    /// <summary>
    /// Indicates default behavior which is leaving the checksum unaltered.
    /// </summary>
    Unaltered = 0,
    /// <summary>
    /// Indicates that a checksum is combined from all given input and hence always will be available.
    /// </summary>
    Combined,
    /// <summary>
    /// Indicates that a checksum is generated from date-time inputs.
    /// </summary>
    Timestamp
}
