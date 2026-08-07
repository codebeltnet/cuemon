using Cuemon.Configuration;

namespace Cuemon.Diagnostics;
/// <summary>
/// Defines options that is related to <see cref="ExceptionDescriptor"/> operations.
/// </summary>
public interface IExceptionDescriptorOptions : IParameterObject
{
    /// <summary>
    /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
    /// </summary>
    /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
    public FaultSensitivityDetails SensitivityDetails { get; set; }
}
