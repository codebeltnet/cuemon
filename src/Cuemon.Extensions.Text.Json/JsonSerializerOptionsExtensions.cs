using System;
using System.Text.Json;

namespace Cuemon.Extensions.Text.Json;
/// <summary>
/// Extension methods for the <see cref="JsonSerializerOptions"/> class.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    /// Copies the options from a <see cref="JsonSerializerOptions"/> instance to a new instance.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to extend.</param>
    /// <param name="setup">The <see cref="JsonSerializerOptions" /> which may be configured.</param>
    /// <returns>A new cloned instance of <paramref name="options"/> with optional altering as specified by the <paramref name="setup"/> delegate.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> cannot be null.
    /// </exception>
    public static JsonSerializerOptions Clone(this JsonSerializerOptions options, Action<JsonSerializerOptions> setup = null)
    {
        Validator.ThrowIfNull(options);
        options = new JsonSerializerOptions(options);
        setup?.Invoke(options);
        return options;
    }

    /// <summary>
    /// Returns the specified <paramref name="name"/> adhering to the underlying <see cref="JsonSerializerOptions.PropertyNamingPolicy"/>.
    /// </summary>
    /// <param name="options">The options from which to apply a property naming policy.</param>
    /// <param name="name">The name to apply to a JSON property.</param>
    /// <returns>When <see cref="JsonSerializerOptions.PropertyNamingPolicy"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="JsonNamingPolicy"/>.</returns>
    /// <remarks>A convenient way of defining the property name according to Microsoft design decisions.</remarks>
    public static string SetPropertyName(this JsonSerializerOptions options, string name)
    {
        return options.PropertyNamingPolicy.DefaultOrConvertName(name);
    }
}
