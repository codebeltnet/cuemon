using System;

namespace Cuemon.Xml.Serialization;
/// <summary>
/// Extension methods for the <see cref="XmlSerializerOptions"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class XmlSerializerOptionsDecoratorExtensions
{
    /// <summary>
    /// Applies the enclosed <see cref="XmlSerializerOptions" /> of the specified <paramref name="decorator" /> to the function delegate <see cref="XmlConvert.DefaultSettings"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{XmlSerializerOptions}" /> to extend.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void ApplyToDefaultSettings(this IDecorator<XmlSerializerOptions> decorator)
    {
        Validator.ThrowIfNull(decorator);
        XmlConvert.DefaultSettings = () => decorator.Inner;
    }
}
