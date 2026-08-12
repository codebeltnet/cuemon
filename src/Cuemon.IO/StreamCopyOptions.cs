using System;
using System.IO;

namespace Cuemon.IO;
/// <summary>
/// Configuration options that is related to <see cref="Stream"/> copy operations.
/// </summary>
public class StreamCopyOptions : DisposableOptions
{
    private int _bufferSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamCopyOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="StreamCopyOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="BufferSize"/></term>
    ///         <description>81920</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public StreamCopyOptions()
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
