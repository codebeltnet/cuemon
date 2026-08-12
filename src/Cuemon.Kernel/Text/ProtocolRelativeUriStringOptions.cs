using System;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Configuration options for concrete implementations of <see cref="IConfigurableParser{TOptions}"/> that is related to parsing a protocol relative URI from a <see cref="string"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class ProtocolRelativeUriStringOptions : IParameterObject
{
    private string _relativeReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolRelativeUriStringOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ProtocolRelativeUriStringOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Protocol"/></term>
    ///         <description><see cref="UriScheme.Https"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="RelativeReference"/></term>
    ///         <description><see cref="Alphanumeric.NetworkPathReference"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ProtocolRelativeUriStringOptions()
    {
        Protocol = UriScheme.Https;
        RelativeReference = Alphanumeric.NetworkPathReference;
    }

    /// <summary>
    /// Gets or sets the protocol to replace the relative reference.
    /// </summary>
    /// <value>The protocol to replace the relative reference.</value>
    public UriScheme Protocol { get; set; }

    /// <summary>
    /// Gets or sets the protocol relative reference that needs to be replaced.
    /// </summary>
    /// <value>The protocol relative reference that needs to be replaced.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public string RelativeReference
    {
        get => _relativeReference;
        set
        {
            Validator.ThrowIfNullOrWhitespace(value);
            _relativeReference = value;
        }
    }
}
