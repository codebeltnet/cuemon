using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Text;

namespace Cuemon.Xml;
/// <summary>
/// Extension methods for the <see cref="Stream"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class StreamDecoratorExtensions
{
    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to an <see cref="XmlReader"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="encoding">The text encoding to use.</param>
    /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
    /// <returns>An <see cref="XmlReader"/> representation of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <remarks>If <paramref name="encoding"/> is null, an <see cref="Encoding"/> object will be attempted resolved by <see cref="TryDetectXmlEncoding"/>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static XmlReader ToXmlReader(this IDecorator<Stream> decorator, Encoding encoding = null, Action<XmlReaderSettings> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        if (encoding == null) { TryDetectXmlEncoding(decorator, out encoding); }
        var value = decorator.Inner;
        if (value.CanSeek) { value.Position = 0; }
        var options = Patterns.CreateInstance(setup);
        return XmlReader.Create(new StreamReader(value, encoding), options);
    }

    /// <summary>
    /// Tries to resolve the <see cref="Encoding"/> level of the XML document from the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="result">When this method returns, it contains the <see cref="Encoding"/> value equivalent to the encoding level of the XML document contained in the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>, if the conversion succeeded, or a null reference if the conversion failed. The conversion fails if the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> is null, does not contain BOM information or does not contain an <see cref="XmlDeclaration"/>.</param>
    /// <returns><c>true</c> if the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> was converted successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool TryDetectXmlEncoding(this IDecorator<Stream> decorator, out Encoding result)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner;
        result = new UTF8Encoding(false);
        if (!ByteOrderMark.TryDetectEncoding(value, out var encoding))
        {
            long startingPosition = -1;
            if (value.CanSeek)
            {
                startingPosition = value.Position;
                value.Position = 0;
            }

            var document = new XmlDocument();
            using (var reader = XmlReader.Create(value))
            {
                document.Load(reader);
            }
            if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                var declaration = (XmlDeclaration)document.FirstChild;
                if (!string.IsNullOrEmpty(declaration.Encoding))
                {
                    result = Encoding.GetEncoding(declaration.Encoding);
                    return true;
                }
            }
            if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); }
        }
        else
        {
            result = encoding;
            return true;
        }
        return false;
    }
}
