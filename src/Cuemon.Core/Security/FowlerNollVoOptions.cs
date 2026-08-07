namespace Cuemon.Security;
/// <summary>
/// Configuration options for <see cref="FowlerNollVoHash"/>.
/// </summary>
public class FowlerNollVoOptions : ConvertibleOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVoOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="FowlerNollVoOptions"/>.
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
    ///         <term><see cref="Algorithm"/></term>
    ///         <description><see cref="FowlerNollVoAlgorithm.Fnv1a"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public FowlerNollVoOptions()
    {
        Algorithm = FowlerNollVoAlgorithm.Fnv1a;
        ByteOrder = Endianness.BigEndian;
    }

    /// <summary>
    /// Gets or sets the algorithm of the Fowler-Noll-Vo hash function.
    /// </summary>
    /// <value>The algorithm of the Fowler-Noll-Vo hash function.</value>
    public FowlerNollVoAlgorithm Algorithm { get; set; }
}
