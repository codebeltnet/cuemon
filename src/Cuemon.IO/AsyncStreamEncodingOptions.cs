using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="Stream"/>.
/// </summary>
public class AsyncStreamEncodingOptions : AsyncDisposableOptions, IEncodingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncStreamEncodingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AsyncStreamEncodingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Preamble"/></term>
    ///         <description><see cref="EncodingOptions.DefaultPreambleSequence"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Encoding"/></term>
    ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncStreamEncodingOptions()
    {
        Encoding = EncodingOptions.DefaultEncoding;
        Preamble = EncodingOptions.DefaultPreambleSequence;
    }

    /// <summary>
    /// Gets or sets the action to take in regards to encoding related preamble sequences.
    /// </summary>
    /// <value>A value that indicates whether to preserve or remove preamble sequences.</value>
    public PreambleSequence Preamble { get; set; }

    /// <summary>
    /// Gets or sets the character encoding to use for the operation.
    /// </summary>
    /// <value>The character encoding to use for the operation.</value>
    public Encoding Encoding { get; set; }
}
