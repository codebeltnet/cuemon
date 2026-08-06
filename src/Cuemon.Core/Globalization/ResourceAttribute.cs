using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Cuemon.Globalization;
/// <summary>
/// Provides a generic way to support localization on attribute decorated methods.
/// </summary>
/// <seealso cref="Attribute" />
public abstract class ResourceAttribute : Attribute
{
    private readonly ConcurrentDictionary<string, PropertyInfo> _propertyInfos = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceAttribute"/> class.
    /// </summary>
    protected ResourceAttribute()
    {
    }

    /// <summary>
    /// Gets or sets the type that contains the resources for looking up localized strings.
    /// </summary>
    /// <value>The type that contains the resources for looking up localized strings.</value>
    public Type ResourceType { get; set; }

    /// <summary>
    /// Returns the value of the specified string resource.
    /// </summary>
    /// <param name="name">The name of the resource to retrieve.</param>
    /// <returns>The value of the resource localized for the callers current UI culture, or <c>null</c> if <paramref name="name"/> cannot be found on the <see cref="ResourceType"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// You must specify a <see cref="ResourceType"/> to perform the actual lookup of localized strings.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The specified <see cref="ResourceType"/> does not contain a resource with the specified <paramref name="name"/>.
    /// </exception>
    protected string GetString(string name)
    {
        Validator.ThrowIfNullOrWhitespace(name);
        if (ResourceType == null) { throw new InvalidOperationException("You must specify a type to perform the actual lookup of localized strings."); }
        var cacheKey = $"{ResourceType.ToString().ToUpperInvariant()}.{name.ToUpperInvariant()}";
        if (!_propertyInfos.TryGetValue(cacheKey, out var property))
        {
            property = ResourceType.GetProperty(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            var getMethod = property?.GetGetMethod(true);
            if (getMethod != null && (getMethod.IsAssembly || getMethod.IsPublic))
            {
                _propertyInfos.TryAdd(cacheKey, property);
            }
        }
        return property == null ? throw new ArgumentException($"The specified type, '{ResourceType.FullName}', does not contain a resource with the specified '{name}'.") : property.GetValue(null, null) as string;
    }
}
