using System;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.Xml.Serialization;
/// <summary>
/// Extension methods for the <see cref="XmlSerializerOptions"/> class.
/// </summary>
public static class XmlSerializerOptionsExtensions
{
    /// <summary>
    /// Applies the specified <paramref name="options"/> to the function delegate <see cref="XmlConvert.DefaultSettings"/>.
    /// </summary>
    /// <param name="options">The <see cref="XmlSerializerOptions"/> to extend.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> cannot be null.
    /// </exception>
    public static void ApplyToDefaultSettings(this XmlSerializerOptions options)
    {
        Validator.ThrowIfNull(options);
        Decorator.Enclose(options).ApplyToDefaultSettings();
    }
}
