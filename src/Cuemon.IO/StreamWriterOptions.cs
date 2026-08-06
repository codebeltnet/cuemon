using System;
using System.Globalization;
using System.IO;
using Cuemon.Text;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="StreamWriter" />.
/// </summary>
public class StreamWriterOptions : StreamEncodingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamWriterOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="StreamWriterOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="AutoFlush"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BufferSize"/></term>
    ///         <description><c>1024</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StreamEncodingOptions.Preamble"/></term>
    ///         <description><see cref="PreambleSequence.Keep"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StreamEncodingOptions.Encoding"/></term>
    ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="FormatProvider"/></term>
    ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="NewLine"/></term>
    ///         <description><see cref="Environment.NewLine"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public StreamWriterOptions()
    {
        AutoFlush = false;
        BufferSize = 1024;
        Encoding = EncodingOptions.DefaultEncoding;
        Preamble = PreambleSequence.Keep;
        FormatProvider = CultureInfo.InvariantCulture;
        NewLine = Environment.NewLine;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="StreamWriter"/> will flush its buffer to the underlying stream after every call to the <c>Write</c> method.
    /// </summary>
    /// <value><c>true</c> to force <see cref="StreamWriter"/> to flush its buffer; otherwise, <c>false</c>.</value>
    public bool AutoFlush { get; set; }

    /// <summary>
    /// Gets or sets the size of the buffer.
    /// </summary>
    /// <value>The size of the buffer in bytes for the <see cref="StreamWriter"/>.</value>
    public int BufferSize { get; set; }

    /// <summary>
    /// Gets or sets the culture-specific formatting information used when writing data with the <see cref="StreamWriter"/>.
    /// </summary>
    /// <value>An <see cref="IFormatProvider"/> that contains the culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</value>
    public IFormatProvider FormatProvider { get; set; }

    /// <summary>
    /// Gets or sets the line terminator string used by the <see cref="StreamWriter"/>.
    /// </summary>
    /// <value>The line terminator string for the <see cref="StreamWriter"/>.</value>
    public string NewLine { get; set; }
}
