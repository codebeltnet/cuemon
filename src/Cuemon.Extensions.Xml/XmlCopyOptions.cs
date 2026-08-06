using System;
using System.Xml;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Configuration options for <see cref="XmlWriter"/>.
/// </summary>
public class XmlCopyOptions : DisposableOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlCopyOptions"/> class.
    /// </summary>
    public XmlCopyOptions()
    {
        WriterSettings = null;
    }

    /// <summary>
    /// Gets or sets the <see cref="XmlWriterSettings"/> which will be applied doing the copying process and need to be configured.
    /// </summary>
    /// <value>The writer settings.</value>
    public Action<XmlWriterSettings> WriterSettings { get; set; }
}
