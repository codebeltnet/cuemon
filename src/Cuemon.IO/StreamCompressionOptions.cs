using System.IO;
using System.IO.Compression;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for compressed <see cref="Stream"/>.
/// </summary>
public class StreamCompressionOptions : StreamCopyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamCompressionOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="StreamCompressionOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Level"/></term>
    ///         <description><see cref="CompressionLevel.Optimal"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public StreamCompressionOptions()
    {
        Level = CompressionLevel.Optimal;
    }

    /// <summary>
    /// Gets or sets the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.
    /// </summary>
    /// <value>The level of the compression.</value>
    public CompressionLevel Level { get; set; }
}
