namespace Cuemon.Security;
/// <summary>
/// Configuration options for <see cref="CyclicRedundancyCheck"/>.
/// </summary>
public class CyclicRedundancyCheckOptions : ConvertibleOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicRedundancyCheckOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="CyclicRedundancyCheckOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="EndianOptions.ByteOrder"/></term>
    ///         <description><see cref="Endianness.BigEndian"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ReflectOutput"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ReflectInput"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public CyclicRedundancyCheckOptions()
    {
        ByteOrder = Endianness.BigEndian; // apparently this is the norm for unix machines and what online converters are treating the result byte[]
        ReflectInput = false;
        ReflectOutput = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether each byte is reflected before being processed.
    /// </summary>
    /// <value><c>true</c> if each byte is reflected before being processed; otherwise, <c>false</c>.</value>
    /// <remarks>More information can be found @ http://www.ross.net/crc/download/crc_v3.txt</remarks>
    public bool ReflectInput { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the final register value is reflected first.
    /// </summary>
    /// <value><c>true</c> if the final register value is reflected first; otherwise, <c>false</c>.</value>
    /// <remarks>More information can be found @ http://www.ross.net/crc/download/crc_v3.txt</remarks>
    public bool ReflectOutput { get; set; }
}
