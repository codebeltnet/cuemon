using System.IO;
using Cuemon.Text;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="StreamReader"/>.
/// </summary>
public class StreamReaderOptions : StreamEncodingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamReaderOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="StreamReaderOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="StreamEncodingOptions.Preamble"/></term>
    ///         <description><see cref="EncodingOptions.DefaultPreambleSequence"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StreamEncodingOptions.Encoding"/></term>
    ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BufferSize"/></term>
    ///         <description><c>81920</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public StreamReaderOptions()
    {
        BufferSize = 81920;
    }

    /// <summary>
    /// Gets or sets the minimum size of the buffer.
    /// </summary>
    /// <value>The minimum size of the buffer.</value>
    public int BufferSize { get; set; }
}
