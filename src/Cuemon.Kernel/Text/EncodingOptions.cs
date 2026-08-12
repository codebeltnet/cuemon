using System;
using System.Text;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Specifies options that is related to the <see cref="System.Text.Encoding"/> class.
/// </summary>
public class EncodingOptions : IEncodingOptions, IParameterObject
{
    private Encoding _encoding;

    /// <summary>
    /// Gets or sets the default preamble action of <see cref="EncodingOptions"/>. Default is <see cref="PreambleSequence.Remove"/>.
    /// </summary>
    /// <value>The default preamble action to use in <see cref="System.Text.Encoding"/> related operations.</value>
    /// <remarks>Warning: changing this value should be thought through carefully as it can change the behavior you have come to expect. Consider using local adjustment instead.</remarks>
    public static PreambleSequence DefaultPreambleSequence { get; set; } = PreambleSequence.Remove;

    /// <summary>
    /// Gets or sets the default encoding of <see cref="EncodingOptions"/>. Default is <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <value>The default encoding to use in <see cref="System.Text.Encoding"/> related operations.</value>
    /// <remarks>Warning: changing this value should be thought through carefully as it can change the behavior you have come to expect. Consider using local adjustment instead.</remarks>
    public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="EncodingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Preamble"/></term>
    ///         <description><see cref="DefaultPreambleSequence"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Encoding"/></term>
    ///         <description><see cref="DefaultEncoding"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public EncodingOptions()
    {
        Preamble = DefaultPreambleSequence;
        Encoding = DefaultEncoding;
    }

    /// <summary>
    /// Gets or sets the action to take in regards to encoding related preamble sequences.
    /// </summary>
    /// <value>A value that indicates whether to preserve or remove preamble sequences.</value>
    public PreambleSequence Preamble { get; set; }

    /// <summary>
    /// Gets or sets the encoding for the operation.
    /// </summary>
    /// <value>The encoding for the operation.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public Encoding Encoding
    {
        get => _encoding;
        set
        {
            Validator.ThrowIfNull(value);
            _encoding = value;
        }
    }
}
