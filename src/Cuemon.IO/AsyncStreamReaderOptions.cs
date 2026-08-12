using System;
using System.IO;
using Cuemon.Text;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="StreamReader"/>.
/// </summary>
public class AsyncStreamReaderOptions : AsyncStreamEncodingOptions
{
    private int _bufferSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncStreamReaderOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AsyncStreamReaderOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="AsyncStreamEncodingOptions.Preamble"/></term>
    ///         <description><see cref="EncodingOptions.DefaultPreambleSequence"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="AsyncStreamEncodingOptions.Encoding"/></term>
    ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BufferSize"/></term>
    ///         <description><c>81920</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncStreamReaderOptions()
    {
        BufferSize = 81920;
    }

    /// <summary>
    /// Gets or sets the size of the buffer.
    /// </summary>
    /// <value>The size of the buffer.</value>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value" /> is lower than or equal to 0.
    /// </exception>
    public int BufferSize
    {
        get => _bufferSize;
        set
        {
            Validator.ThrowIfLowerThanOrEqual(value, 0, nameof(value));
            _bufferSize = value;
        }
    }
}
