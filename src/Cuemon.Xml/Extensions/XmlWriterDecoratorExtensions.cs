using System;
using System.Reflection;
using System.Xml;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Runtime;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Xml;
/// <summary>
/// Extension methods for the <see cref="XmlWriter"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class XmlWriterDecoratorExtensions
{
    /// <summary>
    /// Serializes the specified <paramref name="value"/> into an XML format of the enclosed <see cref="XmlWriter"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{XmlWriter}" /> to extend.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="setup">The <see cref="XmlFormatterOptions"/> which may be configured.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void WriteObject<T>(this IDecorator<XmlWriter> decorator, T value, Action<XmlFormatterOptions> setup = null)
    {
        WriteObject(decorator, value, typeof(T), setup);
    }

    /// <summary>
    /// Serializes the specified <paramref name="value"/> into an XML format of the enclosed <see cref="XmlWriter"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{XmlWriter}" /> to extend.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <param name="setup">The <see cref="XmlFormatterOptions"/> which may be configured.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void WriteObject(this IDecorator<XmlWriter> decorator, object value, Type objectType, Action<XmlFormatterOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var formatter = new XmlFormatter(setup);
        formatter.SerializeToWriter(decorator.Inner, value, objectType);
    }

    /// <summary>
    /// Writes the specified start tag and associates it with the given <paramref name="elementName"/> of the enclosed <see cref="XmlWriter"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{XmlWriter}" /> to extend.</param>
    /// <param name="elementName">The fully qualified name of the element.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void WriteStartElement(this IDecorator<XmlWriter> decorator, XmlQualifiedEntity elementName)
    {
        Validator.ThrowIfNull(decorator);
        decorator.Inner.WriteStartElement(elementName.Prefix, elementName.LocalName, elementName.Namespace);
    }

    /// <summary>
    /// Writes the specified <paramref name="value"/> with the delegate <paramref name="nodeWriter"/> to the enclosed <see cref="XmlWriter"/> of the specified <paramref name="decorator"/>.
    /// If <paramref name="elementName"/> is not null, then the delegate <paramref name="nodeWriter"/> is called from within an encapsulating Start- and End-element.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{XmlWriter}" /> to extend.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="elementName">The optional fully qualified name of the element.</param>
    /// <param name="nodeWriter">The delegate node writer.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void WriteEncapsulatingElementIfNotNull<T>(this IDecorator<XmlWriter> decorator, T value, XmlQualifiedEntity elementName, Action<XmlWriter, T> nodeWriter)
    {
        Validator.ThrowIfNull(decorator);
        var writer = decorator.Inner;
        if (elementName == null)
        {
            nodeWriter(writer, value);
            return;
        }
        WriteStartElement(decorator, elementName);
        nodeWriter(writer, value);
        writer.WriteEndElement();
    }

    /// <summary>
    /// Writes the XML root element to the enclosed <see cref="XmlWriter"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{XmlWriter}" /> to extend.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="treeWriter">The delegate used to write the XML hierarchy.</param>
    /// <param name="rootEntity">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void WriteXmlRootElement<T>(this IDecorator<XmlWriter> decorator, T value, Action<XmlWriter, T, XmlQualifiedEntity> treeWriter, XmlQualifiedEntity rootEntity = null)
    {
        Validator.ThrowIfNull(decorator);
        WriteXmlRootElementCore(decorator.Inner, value, (w, o) => treeWriter(w, value, rootEntity), null, rootEntity);
    }

    internal static void WriteXmlRootElement(this IDecorator<XmlWriter> decorator, object value, Action<XmlWriter, IHierarchy<object>> treeWriter, XmlQualifiedEntity rootEntity = null)
    {
        WriteXmlRootElementCore(decorator.Inner, value, null, treeWriter, rootEntity);
    }

    private static void WriteXmlRootElementCore(XmlWriter writer, object value, Action<XmlWriter, object> treeWriterPublic, Action<XmlWriter, IHierarchy<object>> treeWriterInternal, XmlQualifiedEntity rootEntity = null)
    {
        Validator.ThrowIfNull(writer);
        if (value == null) { return; }
        try
        {
            IHierarchy<object> nodes;
            XmlQualifiedEntity rootElement;
            if (treeWriterInternal == null)
            {
                nodes = new Hierarchy<object>().Add(value);
                rootElement = Decorator.Enclose(nodes).GetXmlQualifiedEntity(rootEntity);
                writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
                treeWriterPublic?.Invoke(writer, value);
            }
            else
            {
                nodes = new HierarchySerializer(value).Nodes;
                rootElement = Decorator.Enclose(nodes).GetXmlQualifiedEntity(rootEntity);
                writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
                treeWriterInternal(writer, nodes);
            }
        }
        catch (Exception ex)
        {
            var innerException = ex;
            if (innerException is OutOfMemoryException) { throw; }
            if (innerException is TargetInvocationException) { innerException = innerException.InnerException; }
            throw ExceptionInsights.Embed(new InvalidOperationException("There is an error in the XML document.", innerException), MethodBase.GetCurrentMethod(), Arguments.ToArray(writer, value));
        }
        writer.WriteEndElement();
        writer.Flush();
    }
}
