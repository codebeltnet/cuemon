using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.Extensions.Runtime;
using Cuemon.Reflection;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml;
/// <summary>
/// Extension methods for the <see cref="IHierarchy{T}"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class HierarchyDecoratorExtensions
{
    /// <summary>
    /// Determines whether the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> implements <see cref="XmlIgnoreAttribute"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <returns>
    /// 	<c>true</c> if the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> implements <see cref="XmlIgnoreAttribute"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasXmlIgnoreAttribute(this IDecorator<IHierarchy<object>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.HasMemberReference && Decorator.Enclose(decorator.Inner.MemberReference).HasAttribute(typeof(XmlIgnoreAttribute));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <returns>
    /// 	<c>true</c> if the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsNodeEnumerable(this IDecorator<IHierarchy<object>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return Decorator.Enclose(decorator.Inner.InstanceType).HasEnumerableImplementation() && (decorator.Inner.InstanceType != typeof(string));
    }

    /// <summary>
    /// Resolves an <see cref="XmlQualifiedEntity"/> from either the specified <paramref name="qualifiedEntity"/> or from the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="qualifiedEntity">The optional <see cref="XmlQualifiedEntity"/> that is part of the equation.</param>
    /// <returns>An <see cref="XmlQualifiedEntity"/> that is either from <paramref name="qualifiedEntity"/>, embedded within <paramref name="decorator"/>, <see cref="XmlRootAttribute"/>, <see cref="XmlElementAttribute"/>, <see cref="XmlAttributeAttribute"/> or resolved from either member name or member type (in that order).</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static XmlQualifiedEntity GetXmlQualifiedEntity(this IDecorator<IHierarchy<object>> decorator, XmlQualifiedEntity qualifiedEntity = null)
    {
        Validator.ThrowIfNull(decorator);

        if (qualifiedEntity != null && !string.IsNullOrWhiteSpace(qualifiedEntity.LocalName)) { return qualifiedEntity; }

        if (decorator.Inner.Instance is XmlQualifiedEntity qre && !string.IsNullOrWhiteSpace(qre.LocalName)) { return qre; }

        var defaultLocalName = decorator.Inner.HasMemberReference
            ? Decorator.Enclose(decorator.Inner.MemberReference.Name).SanitizeXmlElementName()
            : Decorator.Enclose(Decorator.Enclose(decorator.Inner.InstanceType).ToFriendlyName(o => o.ExcludeGenericArguments = true)).SanitizeXmlElementName();


        if (decorator.TryGetXmlRootAttribute(out var rootAttribute))
        {
            if (string.IsNullOrWhiteSpace(rootAttribute.ElementName)) { rootAttribute.ElementName = defaultLocalName; }
            return new XmlQualifiedEntity(rootAttribute);
        }

        if (decorator.TryGetXmlElementAttribute(out var elementAttribute))
        {
            if (string.IsNullOrWhiteSpace(elementAttribute.ElementName)) { elementAttribute.ElementName = defaultLocalName; }
            return new XmlQualifiedEntity(elementAttribute);
        }

        if (decorator.TryGetXmlAttributeAttribute(out var attributeAttribute))
        {
            if (string.IsNullOrWhiteSpace(attributeAttribute.AttributeName)) { attributeAttribute.AttributeName = defaultLocalName; }
            return new XmlQualifiedEntity(attributeAttribute);
        }

        return new XmlQualifiedEntity(Decorator.Enclose(defaultLocalName).SanitizeXmlElementName());
    }

    /// <summary>
    /// Attempts to get an <see cref="XmlTextAttribute"/> from the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="xmlAttribute">When this method returns, contains the <see cref="XmlTextAttribute"/> associated with the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.</param>
    /// <returns><c>true</c> if underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> contains an <see cref="XmlTextAttribute"/>, <c>false</c> otherwise.</returns>
    public static bool TryGetXmlTextAttribute(this IDecorator<IHierarchy<object>> decorator, out XmlTextAttribute xmlAttribute)
    {
        xmlAttribute = decorator.Inner.HasMemberReference ? decorator.Inner.MemberReference.GetCustomAttribute<XmlTextAttribute>(true) : null;
        return xmlAttribute != null;
    }

    /// <summary>
    /// Attempts to get an <see cref="XmlAttributeAttribute"/> from the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="xmlAttribute">When this method returns, contains the <see cref="XmlAttributeAttribute"/> associated with the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.</param>
    /// <returns><c>true</c> if underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> contains an <see cref="XmlAttributeAttribute"/>, <c>false</c> otherwise.</returns>
    public static bool TryGetXmlAttributeAttribute(this IDecorator<IHierarchy<object>> decorator, out XmlAttributeAttribute xmlAttribute)
    {
        xmlAttribute = decorator.Inner.HasMemberReference ? decorator.Inner.MemberReference.GetCustomAttribute<XmlAttributeAttribute>(true) : null;
        return xmlAttribute != null;
    }

    /// <summary>
    /// Attempts to get an <see cref="XmlRootAttribute"/> from the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="xmlAttribute">When this method returns, contains the <see cref="XmlRootAttribute"/> associated with the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.</param>
    /// <returns><c>true</c> if underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> contains an <see cref="XmlRootAttribute"/>, <c>false</c> otherwise.</returns>
    public static bool TryGetXmlRootAttribute(this IDecorator<IHierarchy<object>> decorator, out XmlRootAttribute xmlAttribute)
    {
        xmlAttribute = decorator.Inner.HasMemberReference
            ? decorator.Inner.MemberReference.GetCustomAttribute<XmlRootAttribute>(true)
            : decorator.Inner.InstanceType.GetCustomAttribute<XmlRootAttribute>(true);
        return xmlAttribute != null;
    }

    /// <summary>
    /// Attempts to get an <see cref="XmlElementAttribute"/> from the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="xmlAttribute">When this method returns, contains the <see cref="XmlElementAttribute"/> associated with the underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/>.</param>
    /// <returns><c>true</c> if underlying <see cref="IHierarchy{Object}"/> of the <paramref name="decorator"/> contains an <see cref="XmlElementAttribute"/>, <c>false</c> otherwise.</returns>
    public static bool TryGetXmlElementAttribute(this IDecorator<IHierarchy<object>> decorator, out XmlElementAttribute xmlAttribute)
    {
        xmlAttribute = decorator.Inner.HasMemberReference ? decorator.Inner.MemberReference.GetCustomAttribute<XmlElementAttribute>(true) : null;
        return xmlAttribute != null;
    }

    /// <summary>  
    /// Orders a sequence of <see cref="IHierarchy{T}"/> from the underlying sequence of <see cref="IHierarchy{Object}"/> values of the <paramref name="decorator"/> by nodes having an <see cref="XmlAttributeAttribute"/> decoration.
    /// </summary>
    /// <typeparam name="T">The type of the node represented in the hierarchical structure.</typeparam>
    /// <param name="decorator">The decorator that wraps the sequence of <see cref="IHierarchy{Object}"/> values to extend.</param>
    /// <returns>A sequence of <see cref="IHierarchy{T}"/> that is sorted by nodes having an <see cref="XmlAttributeAttribute"/> decoration first.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IDecorator<IEnumerable<IHierarchy<T>>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var attributes = new List<IHierarchy<T>>();
        var rest = new List<IHierarchy<T>>();
        foreach (var value in decorator.Inner)
        {
            var attribute = value.MemberReference?.GetCustomAttribute<XmlAttributeAttribute>();
            if (attribute != null)
            {
                attributes.Add(value);
            }
            else
            {
                rest.Add(value);
            }
        }
        return attributes.Concat(rest);
    }
}
