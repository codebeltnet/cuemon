using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection;
/// <summary>
/// Extension methods for the <see cref="Type"/> class.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Retrieves a collection that represents all properties defined on the specified <paramref name="source"/> and its inheritance chain.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="PropertyInfo"/> objects of the specified <paramref name="source"/> and its inheritance chain.</returns>
    public static IEnumerable<PropertyInfo> GetAllProperties(this Type source, Action<MemberReflectionOptions> setup = null)
    {
        return Decorator.EncloseToExpose(source).GetAllProperties(setup);
    }

    /// <summary>
    /// Retrieves a collection that represents all fields defined on the specified <paramref name="source"/> and its inheritance chain.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="FieldInfo"/> objects of the specified <paramref name="source"/> and its inheritance chain.</returns>
    public static IEnumerable<FieldInfo> GetAllFields(this Type source, Action<MemberReflectionOptions> setup = null)
    {
        return Decorator.EncloseToExpose(source).GetAllFields(setup);
    }

    /// <summary>
    /// Retrieves a collection that represents all events defined on the specified <paramref name="source"/> and its inheritance chain.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="EventInfo"/> objects of the specified <paramref name="source"/> and its inheritance chain.</returns>
    public static IEnumerable<EventInfo> GetAllEvents(this Type source, Action<MemberReflectionOptions> setup = null)
    {
        return Decorator.EncloseToExpose(source).GetAllEvents(setup);
    }

    /// <summary>
    /// Retrieves a collection that represents all methods defined on the specified <paramref name="source"/> and its inheritance chain.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="MethodInfo"/> objects of the specified <paramref name="source"/> and its inheritance chain.</returns>
    public static IEnumerable<MethodInfo> GetAllMethods(this Type source, Action<MemberReflectionOptions> setup = null)
    {
        return Decorator.EncloseToExpose(source).GetAllMethods(setup);
    }

    /// <summary>
    /// Gets a collection (self-to-derived) of derived / descendant types of the <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains the derived types of the <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetDerivedTypes(this Type source, params Assembly[] assemblies)
    {
        return Decorator.EncloseToExpose(source).GetDerivedTypes(assemblies);
    }

    /// <summary>
    /// Gets a collection (inherited-to-self) of inherited / ancestor types of the <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains the inherited types of the <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetInheritedTypes(this Type source)
    {
        return Decorator.EncloseToExpose(source).GetInheritedTypes();
    }

    /// <summary>
    /// Gets a collection (inherited-to-self-to-derived) of inherited / ancestor and derived / descendant types of the <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> to extend.</param>
    /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains a sorted (base-to-derived) collection of inherited and derived types of the <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetHierarchyTypes(this Type source, params Assembly[] assemblies)
    {
        return Decorator.EncloseToExpose(source).GetHierarchyTypes(assemblies);
    }

    /// <summary>
    /// Loads the embedded resources from the associated <see cref="Assembly"/> of the specified <see cref="Type"/> following the <see cref="ManifestResourceMatch"/> ruleset of <paramref name="match"/>.
    /// </summary>
    /// <param name="source">The source type to load the resource from.</param>
    /// <param name="name">The name of the resource being requested.</param>
    /// <param name="match">The match ruleset to apply.</param>
    /// <returns>A <see cref="Stream"/> representing the loaded resources; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null -or-
    /// <paramref name="name"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="name"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="match"/> was not in the range of valid values.
    /// </exception>
    public static IDictionary<string, Stream> GetEmbeddedResources(this Type source, string name, ManifestResourceMatch match)
    {
        Validator.ThrowIfNull(source);
        return Decorator.Enclose(source.Assembly).GetManifestResources(name, match);
    }

    /// <summary>
    /// Retrieves a collection that represents all the properties defined on a specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to exclude properties on <paramref name="type"/>.</typeparam>
    /// <param name="type">The type that contains the properties to include except those defined on <typeparamref name="T"/>.</param>
    /// <returns>A collection of properties for the specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> cannot be null.
    /// </exception>
    public static IEnumerable<PropertyInfo> GetRuntimePropertiesExceptOf<T>(this Type type)
    {
        return Decorator.EncloseToExpose(type).GetRuntimePropertiesExceptOf<T>();
    }

    /// <summary>
    /// Converts the <see cref="Type"/> to its equivalent string representation.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to extend.</param>
    /// <returns>A string that contains the fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly.</returns>
    public static string ToFullNameIncludingAssemblyName(this Type type)
    {
        return FormattableString.Invariant($"{type.FullName}, {type.GetTypeInfo().Assembly.GetName().Name}");
    }
}
