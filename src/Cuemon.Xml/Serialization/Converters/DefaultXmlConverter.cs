using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.Runtime;
using Cuemon.Reflection;
using Cuemon.Xml.Linq;

namespace Cuemon.Xml.Serialization.Converters;
/// <summary>
/// Provides a default way to convert objects to and from XML.
/// </summary>
public sealed class DefaultXmlConverter : XmlConverter
{
    private const string EnumerableElementName = "Item";
    private const string XmlWriterMethod = "WriteXml";

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultXmlConverter"/> class.
    /// </summary>
    public DefaultXmlConverter(XmlQualifiedEntity rootName, IList<XmlConverter> converters)
    {
        RootName = rootName;
        Converters = converters ?? new List<XmlConverter>();
    }

    private XmlQualifiedEntity RootName { get; }

    private IList<XmlConverter> Converters { get; }

    /// <summary>
    /// Converts an object into its XML representation.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> stream to which the object is serialized.</param>
    /// <param name="value">The object to convert.</param>
    /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
    /// <exception cref="InvalidOperationException">There is an error in the XML document.</exception>
    public override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
    {
        Decorator.Enclose(writer).WriteXmlRootElement(value, WriteXmlNodes, elementName ?? RootName);
    }

    /// <summary>
    /// Generates an object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> stream from which the object is deserialized.</param>
    /// <param name="objectType">The <see cref="Type"/> of the object to generate.</param>
    /// <returns>The generated (deserialized) object.</returns>
    public override object ReadXml(XmlReader reader, Type objectType)
    {
        Validator.ThrowIfNull(reader);
        Validator.ThrowIfNull(objectType);
        if (Decorator.Enclose(objectType).HasEnumerableImplementation() && objectType != typeof(string))
        {
            if (Decorator.Enclose(objectType).HasDictionaryImplementation())
            {
                return ParseReadXmlDictionary(reader, objectType);
            }
            return ParseReadXmlEnumerable(reader, objectType);
        }
        var valueTypeInfo = objectType.GetTypeInfo();
        if (valueTypeInfo.IsPrimitive ||
            objectType == typeof(string) ||
            objectType == typeof(Guid) ||
            objectType == typeof(decimal))
        {
            return ParseReadXmlSimple(reader, objectType);
        }
        return ParseReadXmlDefault(reader, objectType);
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
    /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    private static object ParseReadXmlDictionary(XmlReader reader, Type valueType)
    {
        var values = new Dictionary<string, string>();
        var hierarchy = Decorator.Enclose(reader).ToHierarchy();
        var items = Decorator.Enclose(hierarchy).Find(h => h.Instance.Name == EnumerableElementName && h.Depth == 1).ToList();
        foreach (var item in items)
        {
            if (item.HasChildren)
            {
                try
                {
                    var key = item.GetChildren().SingleOrDefault();
                    var value = key?.GetChildren().SingleOrDefault();
                    if (value != null)
                    {
                        values.Add(key.Instance.Value.ToString(), value.Instance.Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException("Deserialization of complex objects is not supported in this version.", ex);
                }
            }
        }

        var dictionaryType = valueType.GetGenericArguments().Length > 0 ? valueType.GetGenericArguments() : new[] { typeof(object), typeof(object) };
        var dictionary = typeof(Dictionary<,>).MakeGenericType(dictionaryType);
        var castedValues = values.Select(pair => new MutableTuple<object, object>(Decorator.Enclose(pair.Key).ChangeType(dictionaryType[0]), Decorator.Enclose(pair.Value).ChangeType(dictionaryType[1]))).ToList();
        var instance = Activator.CreateInstance(dictionary);
        var addMethod = valueType.GetMethod("Add");
        foreach (var item in castedValues)
        {
            addMethod.Invoke(instance, new[] { item.Arg1, item.Arg2 });
        }
        return instance;
    }

    private static object ParseReadXmlEnumerable(XmlReader reader, Type valueType)
    {
        var values = new List<KeyValuePair<string, string>>();
        var hierarchy = Decorator.Enclose(reader).ToHierarchy();
        var items = Decorator.Enclose(hierarchy).Find(h => h.Instance.Name == EnumerableElementName && h.Depth == 1).ToList();
        if (items.FirstOrDefault()?.HasChildren ?? false) { throw new NotSupportedException("Deserialization of complex objects is not supported in this version."); }
        values.AddRange(items.Select(h => new KeyValuePair<string, string>(h.Instance.Name, h.Instance.Value.ToString())));

        var enumerableType = valueType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
        var listEnumerable = typeof(List<>).MakeGenericType(enumerableType);
        var castedValues = values.Where(pair => pair.Key == EnumerableElementName).Select(pair => Decorator.Enclose(pair.Value).ChangeType(enumerableType)).ToList();
        var instance = Activator.CreateInstance(listEnumerable);
        var addMethod = valueType.GetMethod("Add");
        foreach (var item in castedValues)
        {
            addMethod.Invoke(instance, new[] { item });
        }
        return instance;
    }

    private static object ParseReadXmlSimple(XmlReader reader, Type valueType)
    {
        string simpleValue = null;
        try
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        simpleValue = reader.Value;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new NotSupportedException("Deserialization of complex objects is not supported in this version.", ex);
        }
        return TypeDescriptor.GetConverter(valueType).ConvertFromInvariantString(simpleValue);
    }

    private static object ParseReadXmlDefault(XmlReader reader, Type valueType)
    {
        var key = "";
        var values = new Dictionary<string, string>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Attribute:
                    values.Add(reader.Name, reader.Value);
                    while (reader.MoveToNextAttribute()) { goto case XmlNodeType.Attribute; }
                    reader.MoveToElement();
                    break;
                case XmlNodeType.Element:
                    if (reader.Depth == 0) { continue; }
                    key = reader.Name;
                    if (reader.HasAttributes && reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                    break;
                case XmlNodeType.CDATA:
                case XmlNodeType.Text:
                    Decorator.Enclose(values).AddOrUpdate(key, reader.Value);
                    break;
            }
        }

        var hasDefaultCtor = false;
        var constructors = valueType.GetConstructors(new MemberReflection(excludeStatic: true)).ToList();
        var properties = valueType.GetProperties(new MemberReflection(excludeStatic: true)).Where(info => info.CanWrite).ToDictionary(info => info.Name);
        var propertyNames = properties.Select(info => info.Key).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).ToList();

        var args = new List<object>();
        foreach (var ctr in constructors)
        {
            var arguments = ctr.GetParameters();
            var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
            if (arguments.Length == argumentsLength)
            {
                if (!hasDefaultCtor && argumentsLength == 0) { hasDefaultCtor = true; }
                foreach (var arg in arguments)
                {
                    args.Add(Decorator.Enclose(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value).ChangeType(arg.ParameterType));
                }
                break;
            }
        }

        if (args.Count == 0)
        {
            var staticMethods = valueType.GetMethods(new MemberReflection(excludeInheritancePath: true)).Where(info => info.ReturnType == valueType && info.IsStatic && !info.IsSpecialName).ToList();
            foreach (var method in staticMethods)
            {
                var arguments = method.GetParameters();
                var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                if (arguments.Length == argumentsLength)
                {
                    foreach (var arg in arguments)
                    {
                        args.Add(Decorator.Enclose(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value).ChangeType(arg.ParameterType));
                    }
                    return method.Invoke(null, args.ToArray());
                }
            }
            if (!hasDefaultCtor) { throw new SerializationException("Unable to find a suitable constructor or static method for deserialization."); }
        }

        var instance = Activator.CreateInstance(valueType, args.ToArray());
        foreach (var propertyName in propertyNames)
        {
            var property = properties[propertyName];
            property.SetValue(instance, Decorator.Enclose(values.First(pair => pair.Key == propertyName).Value).ChangeType(property.PropertyType));
        }
        return instance;
    }

    private void WriteXmlNodes(XmlWriter writer, IHierarchy<object> node)
    {
        if (Decorator.Enclose(node).HasXmlIgnoreAttribute()) { return; }

        var writerMethod = node.InstanceType.GetMethod(XmlWriterMethod, new MemberReflection(excludeStatic: true));
        var useWriterMethod = (writerMethod != null) && Decorator.Enclose(node.InstanceType).HasInterfaces(typeof(IXmlSerializable));

        if (useWriterMethod)
        {
            writerMethod.Invoke(node.Instance, new object[] { writer });
        }
        else
        {
            Condition.FlipFlop(node.HasChildren, WriteXmlChildren, WriteXmlValue, writer, node);
        }
    }

    private void WriteXmlValue(XmlWriter writer, IHierarchy<object> node)
    {
        if (Decorator.Enclose(node).IsNodeEnumerable()) { return; }

        var hasTextAttribute = Decorator.Enclose(node).TryGetXmlTextAttribute(out _);
        var qualifiedEntity = Decorator.Enclose(node).GetXmlQualifiedEntity();
        var hasElementAttribute = qualifiedEntity.HasXmlElementDecoration;

        if (!qualifiedEntity.HasXmlAttributeDecoration && !qualifiedEntity.HasXmlElementDecoration && !hasTextAttribute)
        {
            hasElementAttribute = true; // default serialization value for legacy Cuemon
            var isType = node.Instance is Type;
            var nodeType = isType ? (Type)node.Instance : node.InstanceType;
            if ((!Decorator.Enclose(nodeType).IsComplex() || isType) && !node.HasChildren && isType)
            {
                if (!node.HasParent)
                {
                    hasElementAttribute = false;
                }
                hasTextAttribute = true;
            }
        }

        var value = Wrapper.ParseInstance(node);
        if (qualifiedEntity.HasXmlAttributeDecoration)
        {
            writer.WriteAttributeString(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace, value);
        }
        else if (hasElementAttribute)
        {
            WriteXmlElement(writer, node, qualifiedEntity, value);
        }
        else if (hasTextAttribute) // catch all (eg. non-decorated properties or XmlTextAttribute)
        {
            writer.WriteString(value);
        }
    }

    private static void WriteXmlElement(XmlWriter writer, IHierarchy<object> node, XmlQualifiedEntity qualifiedEntity, string value)
    {
        if (node.HasMemberReference)
        {
            writer.WriteElementString(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace, value);
        }
        else
        {
            if (Decorator.Enclose(value).IsXmlString())
            {
                writer.WriteCData(value);
            }
            else
            {
                writer.WriteString(value);
            }
        }
    }

    private void WriteXmlChildren(XmlWriter writer, IHierarchy<object> node)
    {
        foreach (var childNode in Decorator.Enclose(node.GetChildren()).OrderByXmlAttributes())
        {
            if (Decorator.Enclose(childNode).HasXmlIgnoreAttribute()) { continue; }
            if (!childNode.InstanceType.GetTypeInfo().IsValueType && childNode.Instance == null) { continue; }
            if (SkipIfNullOrEmptyEnumerable(childNode)) { continue; }
            var qualifiedEntity = Decorator.Enclose(childNode).GetXmlQualifiedEntity();
            if (qualifiedEntity.HasXmlAttributeDecoration && node.HasMemberReference)
            {
                writer.WriteAttributeString(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace, Wrapper.ParseInstance(childNode));
            }
            else
            {
                WriteXmlChildrenEncapsulated(writer, childNode, qualifiedEntity);
            }
        }
    }

    private static bool SkipIfNullOrEmptyEnumerable(IHierarchy<object> childNode)
    {
        if (Decorator.Enclose(childNode.InstanceType).HasEnumerableImplementation() && childNode.InstanceType != typeof(string) && !Decorator.Enclose(childNode.InstanceType).HasDictionaryImplementation())
        {
            if (childNode.Instance is not IEnumerable i || !i.Cast<object>().Any()) { return true; }
        }
        return false;
    }

    private void WriteXmlChildrenEncapsulated(XmlWriter writer, IHierarchy<object> childNode, XmlQualifiedEntity qualifiedEntity)
    {
        // Determine if there is a specific converter for this child node type
        var converter = Decorator.Enclose(Converters).FirstOrDefaultWriterConverter(childNode.InstanceType);

        // Only encapsulate (write a surrounding start/end element) when there is no dedicated converter
        // for the child node. If a converter exists it is responsible for writing the proper elements
        // (to avoid duplicate wrapping such as <Children><Children>...)
        var encapsulate = childNode.HasChildren && Decorator.Enclose(childNode.InstanceType).IsComplex() && converter == null;
        if (encapsulate) { Decorator.Enclose(writer).WriteStartElement(qualifiedEntity); }

        if (converter != null && !qualifiedEntity.HasXmlAttributeDecoration)
        {
            converter.WriteXml(writer, childNode.Instance, qualifiedEntity);
        }
        else
        {
            WriteXmlNodes(writer, childNode);
        }

        if (encapsulate) { writer.WriteEndElement(); }
    }
}
