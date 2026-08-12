using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection;
/// <summary>
/// Extension methods for the <see cref="Assembly"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class AssemblyDecoratorExtensions
{
    /// <summary>
    /// Determines whether the underlying <see cref="Assembly"/> of the <paramref name="decorator"/> is a debug build.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Assembly"/> of the <paramref name="decorator"/> is a debug build; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsDebugBuild(this IDecorator<Assembly> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var debuggingFlags = GetDebuggingFlags(decorator.Inner);
        var isDebug = debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.Default) ||
                      debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.DisableOptimizations) ||
                      debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.EnableEditAndContinue);
        var isJitTrackingEnabled = (debuggingFlags & DebuggableAttribute.DebuggingModes.Default) != 0;
        return isDebug || isJitTrackingEnabled;
    }

    /// <summary>
    /// Gets the types contained within the underlying <see cref="Assembly"/> of this instance.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="namespaceFilter">The filter to limit the types by namespace.</param>
    /// <param name="typeFilter">The filter to limit the types by a specific type.</param>
    /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filters, from the underlying <see cref="Assembly"/> of this instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetTypes(this IDecorator<Assembly> decorator, string namespaceFilter = null, Type typeFilter = null)
    {
        Validator.ThrowIfNull(decorator);
        var hasNamespaceFilter = !string.IsNullOrEmpty(namespaceFilter);
        var hasTypeFilter = (typeFilter != null);
        var types = decorator.Inner.GetTypes() as IEnumerable<Type>;
        if (hasNamespaceFilter || hasTypeFilter)
        {
            if (hasNamespaceFilter) { types = types.Where(type => type.Namespace != null && type.Namespace.Equals(namespaceFilter, StringComparison.OrdinalIgnoreCase)); }
            if (hasTypeFilter)
            {
                types = typeFilter.IsInterface ? types.Where(type => Decorator.Enclose(type).HasInterfaces(typeFilter)) : types.Where(type => Decorator.Enclose(type).HasTypes(typeFilter));
            }
        }
        return types;
    }

    /// <summary>
    /// Returns a <see cref="VersionResult"/> that represents the <see cref="AssemblyVersionAttribute"/> of the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="VersionResult"/> that represents the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static VersionResult GetAssemblyVersion(this IDecorator<Assembly> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return new VersionResult(decorator.Inner.GetName().Version);
    }

    /// <summary>
    /// Returns a <see cref="VersionResult"/> that represents the <see cref="AssemblyFileVersionAttribute"/> of the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="VersionResult"/> that represents the file version of the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>; null if no <see cref="AssemblyFileVersionAttribute"/> could be retrieved.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static VersionResult GetFileVersion(this IDecorator<Assembly> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var version = decorator.Inner.GetCustomAttribute<AssemblyFileVersionAttribute>();
        return new VersionResult(version.Version);
    }

    /// <summary>
    /// Returns a <see cref="VersionResult"/> that represents the <see cref="AssemblyInformationalVersionAttribute"/> of the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="VersionResult"/> that represents the product version of the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>; null if no <see cref="AssemblyInformationalVersionAttribute"/> could be retrieved.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static VersionResult GetProductVersion(this IDecorator<Assembly> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var version = decorator.Inner.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return new VersionResult(version.InformationalVersion);
    }

    /// <summary>
    /// Loads the embedded resources from the underlying <see cref="Assembly"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="name">The case-sensitive name of the resource being requested.</param>
    /// <param name="match">The ruleset that defines the match to apply.</param>
    /// <returns>An <see cref="IDictionary{TKey,TValue}"/> that contains the result of <paramref name="match"/>.</returns>
    /// <remarks>The result returned can have null values if no resources were specified during compilation or if the resource is not visible to the caller.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="name"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="name"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="match"/> was not in the range of valid values.
    /// </exception>
    /// <seealso cref="Assembly.GetManifestResourceStream(string)"/>
    /// <seealso cref="Assembly.GetManifestResourceNames()"/>
    public static IDictionary<string, Stream> GetManifestResources(this IDecorator<Assembly> decorator, string name, ManifestResourceMatch match = default)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNullOrWhitespace(name);
        var resources = new Dictionary<string, Stream>();
        switch (match)
        {
            case ManifestResourceMatch.Name:
                var rn = decorator.Inner.GetManifestResourceStream(name);
                if (rn != null) { resources.Add(name, rn); }
                break;
            default:
                var resourceNames = decorator.Inner.GetManifestResourceNames();
                switch (match)
                {
                    case ManifestResourceMatch.ContainsName:
                        AddResourcesWhenContainsName(resources, resourceNames, name, decorator.Inner);
                        break;
                    case ManifestResourceMatch.Extension:
                        AddResourcesWhenExtensionPredicate(resources, resourceNames, name, decorator.Inner, (extension, matchExtension) => string.Equals(extension, Path.GetExtension(name), StringComparison.OrdinalIgnoreCase));
                        break;
                    case ManifestResourceMatch.ContainsExtension:
                        AddResourcesWhenExtensionPredicate(resources, resourceNames, name, decorator.Inner, (extension, matchExtension) => extension.IndexOf(matchExtension, StringComparison.OrdinalIgnoreCase) != -1);
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(match), (int)match, typeof(ManifestResourceMatch));
                }
                break;
        }
        return resources;
    }

    private static void AddResourcesWhenContainsName(Dictionary<string, Stream> resources, string[] resourceNames, string name, Assembly assembly)
    {
        foreach (var resourceName in resourceNames)
        {
            if (resourceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1)
            {
                resources.Add(resourceName, assembly.GetManifestResourceStream(resourceName));
            }
        }
    }

    private static void AddResourcesWhenExtensionPredicate(Dictionary<string, Stream> resources, string[] resourceNames, string name, Assembly assembly, Func<string, string, bool> predicate)
    {
        foreach (var resourceName in resourceNames)
        {
            var extension = Path.GetExtension(resourceName).ToUpperInvariant();
            if (predicate(extension, name))
            {
                resources.Add(resourceName, assembly.GetManifestResourceStream(resourceName));
            }
        }
    }

    private static DebuggableAttribute.DebuggingModes GetDebuggingFlags(Assembly assembly)
    {
        var attributes = assembly.CustomAttributes;
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeType == typeof(DebuggableAttribute))
            {
                var debugModesArgument = attribute.ConstructorArguments.First();
                return (DebuggableAttribute.DebuggingModes)Enum.Parse(typeof(DebuggableAttribute.DebuggingModes), debugModesArgument.Value.ToString());
            }
        }
        return DebuggableAttribute.DebuggingModes.None;
    }
}
