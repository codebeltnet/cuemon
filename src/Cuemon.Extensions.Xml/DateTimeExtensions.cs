using System;
using System.Xml;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Extension methods for the <see cref="DateTime"/> struct.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="value"/> to a <see cref="string"/> using the <paramref name="serializationMode"/> specified.
    /// </summary>
    /// <param name="value">The <see cref="DateTime"/> value to convert.</param>
    /// <param name="serializationMode">One of the <see cref="XmlDateTimeSerializationMode"/> values that specify how to treat the <see cref="DateTime"/> <paramref name="value"/>.</param>
    /// <returns>A <see cref="string" /> equivalent of the <see cref="DateTime"/> <paramref name="value"/>.</returns>
    public static string ToString(this DateTime value, XmlDateTimeSerializationMode serializationMode)
    {
        return XmlConvert.ToString(value, serializationMode);
    }
}
