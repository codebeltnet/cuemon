using System;
using System.Xml;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> array.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts the given <paramref name="value"/> to an <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to extend.</param>
        /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
        /// <returns>An <see cref="XmlReader"/> representation of <paramref name="value"/>.</returns>
        public static XmlReader ToXmlReader(this byte[] value, Action<XmlReaderSettings> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).ToStream().ToXmlReader(setup: setup);
        }
    }
}