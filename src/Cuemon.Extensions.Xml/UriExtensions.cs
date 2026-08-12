using System;
using System.Xml;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Extension methods for the <see cref="Uri"/> class.
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Converts the given <paramref name="value"/> to an <see cref="XmlReader"/>.
    /// </summary>
    /// <param name="value">The <see cref="Uri"/> to extend.</param>
    /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
    /// <returns>An <see cref="XmlReader"/> representation of <paramref name="value"/>.</returns>
    public static XmlReader ToXmlReader(this Uri value, Action<XmlReaderSettings> setup = null)
    {
        Validator.ThrowIfNull(value);
        var options = Patterns.CreateInstance(setup);
        return XmlReader.Create(value.ToString(), options);
    }
}
