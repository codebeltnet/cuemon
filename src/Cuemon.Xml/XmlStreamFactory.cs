using System;
using System.IO;
using System.Xml;

namespace Cuemon.Xml;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Stream"/> instances.
/// </summary>
public static class XmlStreamFactory
{
    /// <summary>
    /// Creates and returns an XML stream by the specified delegate <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
    /// <param name="setup">The <see cref="XmlWriterSettings"/> which may be configured.</param>
    /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
    public static Stream CreateStream(Action<XmlWriter> writer, Action<XmlWriterSettings> setup = null)
    {
        var options = Patterns.CreateInstance(setup);
        return Patterns.SafeInvoke(() => new MemoryStream(), ms =>
        {
            using (var w = XmlWriter.Create(ms, options))
            {
                writer(w);
                w.Flush();
            }
            ms.Flush();
            ms.Position = 0;
            return ms;
        }, exception => throw new InvalidOperationException("There is an error in the XML document.", exception));
    }
}
