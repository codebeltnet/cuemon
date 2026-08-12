using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cuemon.Extensions.Runtime;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Extension methods for the <see cref="IHierarchy{T}"/> interface.
/// </summary>
public static class HierarchyExtensions
{
    /// <summary>
    /// Determines whether the <paramref name="hierarchy"/> implements <see cref="XmlIgnoreAttribute"/>.
    /// </summary>
    /// <param name="hierarchy">The <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <returns>
    /// 	<c>true</c> if the <see cref="IHierarchy{Object}"/> implements <see cref="XmlIgnoreAttribute"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hierarchy"/> cannot be null.
    /// </exception>
    public static bool HasXmlIgnoreAttribute(this IHierarchy<object> hierarchy)
    {
        Validator.ThrowIfNull(hierarchy);
        return Decorator.Enclose(hierarchy).HasXmlIgnoreAttribute();
    }

    /// <summary>
    /// Determines whether the <paramref name="hierarchy"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>.
    /// </summary>
    /// <param name="hierarchy">The <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <returns>
    /// 	<c>true</c> if the <see cref="IHierarchy{Object}"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/> and is not a <see cref="string"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hierarchy"/> cannot be null.
    /// </exception>
    public static bool IsNodeEnumerable(this IHierarchy<object> hierarchy)
    {
        Validator.ThrowIfNull(hierarchy);
        return Decorator.Enclose(hierarchy).IsNodeEnumerable();
    }

    /// <summary>
    /// Resolves an <see cref="XmlQualifiedEntity"/> from either the specified <paramref name="qualifiedEntity"/> or from the <see cref="IHierarchy{Object}"/>.
    /// </summary>
    /// <param name="hierarchy">The <see cref="IHierarchy{Object}"/> to extend.</param>
    /// <param name="qualifiedEntity">The optional <see cref="XmlQualifiedEntity"/> that is part of the equation.</param>
    /// <returns>An <see cref="XmlQualifiedEntity"/> that is either from <paramref name="qualifiedEntity"/>, embedded within <paramref name="hierarchy"/>, <see cref="XmlRootAttribute"/>, <see cref="XmlElementAttribute"/>, <see cref="XmlAttributeAttribute"/> or resolved from either member name or member type (in that order).</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hierarchy"/> cannot be null.
    /// </exception>
    public static XmlQualifiedEntity GetXmlQualifiedEntity(this IHierarchy<object> hierarchy, XmlQualifiedEntity qualifiedEntity = null)
    {
        Validator.ThrowIfNull(hierarchy);
        return Decorator.Enclose(hierarchy).GetXmlQualifiedEntity(qualifiedEntity);
    }

    /// <summary>  
    /// Orders a sequence of <see cref="IHierarchy{T}"/> from <paramref name="hierarchies"/> by nodes having an <see cref="XmlAttributeAttribute"/> decoration.
    /// </summary>
    /// <typeparam name="T">The type of the node represented in the hierarchical structure.</typeparam>
    /// <param name="hierarchies">The sequence of <see cref="IHierarchy{Object}"/> values to extend.</param>
    /// <returns>A sequence of <see cref="IHierarchy{T}"/> that is sorted by nodes having an <see cref="XmlAttributeAttribute"/> decoration first.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hierarchies"/> cannot be null.
    /// </exception>
    public static IEnumerable<IHierarchy<T>> OrderByXmlAttributes<T>(this IEnumerable<IHierarchy<T>> hierarchies)
    {
        Validator.ThrowIfNull(hierarchies);
        return Decorator.Enclose(hierarchies).OrderByXmlAttributes();
    }
}
