using System;
using System.IO;
using Cuemon.Configuration;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="FileInfo"/>.
/// </summary>
public class FileInfoOptions : IParameterObject
{
    private int _bytesToRead;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileInfoOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="FileInfoOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="BytesToRead"/></term>
    ///         <description>0</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public FileInfoOptions()
    {
        BytesToRead = 0;
    }

    /// <summary>
    /// Gets or sets the amount of bytes to read from a file.
    /// </summary>
    /// <value>The amount of bytes to read from a file.</value>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value" /> is lower than 0.
    /// </exception>
    public int BytesToRead
    {
        get => _bytesToRead;
        set
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            _bytesToRead = value;
        }
    }
}
