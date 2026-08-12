using System;
using System.Xml.Serialization;

namespace Cuemon.Xml.Serialization;
/// <summary>
/// A class designed to help assure qualified names in XML serializations.
/// </summary>
public sealed class XmlQualifiedEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
    public XmlQualifiedEntity(XmlElementAttribute attribute) : this(ValidateArguments(attribute).ElementName, ValidateArguments(attribute).Namespace)
    {
        HasXmlElementDecoration = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
    public XmlQualifiedEntity(XmlAttributeAttribute attribute) : this(ValidateArguments(attribute).AttributeName, ValidateArguments(attribute).Namespace)
    {
        HasXmlAttributeDecoration = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
    public XmlQualifiedEntity(XmlRootAttribute attribute) : this(ValidateArguments(attribute).ElementName, ValidateArguments(attribute).Namespace)
    {
        HasXmlRootDecoration = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
    public XmlQualifiedEntity(XmlAnyElementAttribute attribute) : this(ValidateArguments(attribute).Name, ValidateArguments(attribute).Namespace)
    {
        HasXmlAnyElementDecoration = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="localName">The local name of the entity.</param>
    public XmlQualifiedEntity(string localName) : this(localName, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="localName">The local name of the entity.</param>
    /// <param name="ns">The namespace URI to associate with the entity.</param>
    public XmlQualifiedEntity(string localName, string ns) : this(null, localName, ns)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
    /// </summary>
    /// <param name="prefix">The namespace prefix of the entity.</param>
    /// <param name="localName">The local name of the entity.</param>
    /// <param name="ns">The namespace URI to associate with the entity.</param>
    public XmlQualifiedEntity(string prefix, string localName, string ns)
    {
        Prefix = prefix;
        LocalName = localName;
        Namespace = ns;
    }

    /// <summary>
    /// Gets a value indicating whether this instance was constructed with an <see cref="XmlAttributeAttribute"/> decoration.
    /// </summary>
    /// <value><c>true</c> if this instance was constructed with an <see cref="XmlAttributeAttribute"/> decoration; otherwise, <c>false</c>.</value>
    public bool HasXmlAttributeDecoration { get; }

    /// <summary>
    /// Gets a value indicating whether this instance was constructed with an <see cref="XmlElementAttribute"/> decoration.
    /// </summary>
    /// <value><c>true</c> if this instance was constructed with an <see cref="XmlElementAttribute"/> decoration; otherwise, <c>false</c>.</value>
    public bool HasXmlElementDecoration { get; }

    /// <summary>
    /// Gets a value indicating whether this instance was constructed with an <see cref="XmlAnyAttributeAttribute"/> decoration.
    /// </summary>
    /// <value><c>true</c> if this instance was constructed with an <see cref="XmlAnyAttributeAttribute"/> decoration; otherwise, <c>false</c>.</value>
    public bool HasXmlAnyElementDecoration { get; }

    /// <summary>
    /// Gets a value indicating whether this instance was constructed with an <see cref="XmlRootAttribute"/> decoration.
    /// </summary>
    /// <value><c>true</c> if this instance was constructed with an <see cref="XmlRootAttribute"/> decoration; otherwise, <c>false</c>.</value>
    public bool HasXmlRootDecoration { get; }

    /// <summary>
    /// Gets the local name of the entity.
    /// </summary>
    /// <value>The local name of the entity.</value>
    public string LocalName { get; private set; }

    /// <summary>
    /// Gets the namespace URI to associate with the entity.
    /// </summary>
    /// <value>The namespace URI to associate with the entity.</value>
    public string Namespace { get; private set; }

    /// <summary>
    /// Gets the namespace prefix of the entity.
    /// </summary>
    /// <value>The namespace prefix of the entity.</value>
    public string Prefix { get; private set; }

    private static T ValidateArguments<T>(T attribute) where T : Attribute
    {
        return Validator.CheckParameter(attribute, () => Validator.ThrowIfNull(attribute));
    }
}
