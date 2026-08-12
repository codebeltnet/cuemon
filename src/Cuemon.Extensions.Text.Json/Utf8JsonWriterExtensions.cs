using System.Text.Json;

namespace Cuemon.Extensions.Text.Json;
/// <summary>
/// Extension methods for the <see cref="Utf8JsonWriter"/> class.
/// </summary>
public static class Utf8JsonWriterExtensions
{
    /// <summary>
    /// Serializes the specified <paramref name="value"/> and writes the JSON structure using the specified <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON structure.</param>
    /// <param name="value">The <see cref="object"/> to serialize.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public static void WriteObject(this Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
