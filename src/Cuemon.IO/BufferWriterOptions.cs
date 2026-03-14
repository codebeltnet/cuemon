#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER
using System.Buffers;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// Configuration options for <see cref="IBufferWriter{T}" />.
    /// </summary>
    public class BufferWriterOptions : StreamEncodingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferWriterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="BufferWriterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="BufferSize"/></term>
        ///         <description><c>256</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamEncodingOptions.Preamble"/></term>
        ///         <description><see cref="PreambleSequence.Keep"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamEncodingOptions.Encoding"/></term>
        ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public BufferWriterOptions()
        {
            Encoding = EncodingOptions.DefaultEncoding;
            Preamble = PreambleSequence.Keep;
            BufferSize = 256;
        }

        /// <summary>
        /// Gets or sets the minimum capacity with which to initialize the underlying buffer.
        /// </summary>
        /// <value>The initial size of the buffer in <see cref="IBufferWriter{T}"/>.</value>
        public int BufferSize { get; set; }
    }
}
#endif
