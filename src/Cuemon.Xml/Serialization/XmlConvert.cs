using System;

namespace Cuemon.Xml.Serialization;
/// <summary>
/// Provides methods for converting between .NET types and XML types.
/// </summary>
public static class XmlConvert
{
    /// <summary>
    /// Gets or sets a function delegate that creates default <see cref="XmlSerializerOptions"/>.
    /// </summary>
    /// <value>The function delegate which provides default settings for <seealso cref="XmlConvert"/> implementations.</value>
    public static Func<XmlSerializerOptions> DefaultSettings { get; set; }
}
