using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json;
/// <summary>
/// Represents the <c>Write</c> method of <see cref="JsonConverter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
/// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
/// <param name="value">The value to convert to JSON.</param>
/// <param name="options">An object that specifies serialization options to use.</param>
public delegate void Utf8JsonWriterAction<in T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
