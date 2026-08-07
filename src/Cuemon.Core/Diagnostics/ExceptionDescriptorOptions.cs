namespace Cuemon.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="ExceptionDescriptor"/> operations.
/// </summary>
public class ExceptionDescriptorOptions : IExceptionDescriptorOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDescriptorOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ExceptionDescriptorOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="SensitivityDetails"/></term>
    ///         <description><see cref="FaultSensitivityDetails.None"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ExceptionDescriptorOptions()
    {
        SensitivityDetails = FaultSensitivityDetails.None;
    }

    /// <summary>
    /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
    /// </summary>
    /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
    public FaultSensitivityDetails SensitivityDetails { get; set; }
}
