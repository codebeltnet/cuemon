using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json.Converters;
/// <summary>
/// Provides a <see cref="DateTime"/> converter that can be configured like the Newtonsoft.JSON equivalent.
/// </summary>
/// <seealso cref="JsonConverter{DateTime}" />
public class DateTimeConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeConverter"/> class.
    /// </summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    public DateTimeConverter(string format = "O", CultureInfo provider = null)
    {
        Format = format;
        Provider = provider;
    }

    private string Format { get; }

    private CultureInfo Provider { get; }

    /// <summary>
    /// Reads and converts the JSON to <see cref="DateTime"/>.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    /// <returns>The converted value.</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.TryParse(reader.GetString(), Provider, DateTimeStyles.RoundtripKind, out var value)
            ? value
            : reader.GetDateTime(); // official fallback incl. possible thrown exceptions
    }

    /// <summary>
    /// Write the value as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, Provider));
    }
}
