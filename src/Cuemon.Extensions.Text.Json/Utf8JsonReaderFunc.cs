using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json;
/// <summary>
/// Represents the <c>Read</c> method of <see cref="JsonConverter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
/// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
/// <param name="typeToConvert">The type to convert.</param>
/// <param name="options">An object that specifies serialization options to use.</param>
/// <returns>The converted value.</returns>
public delegate T Utf8JsonReaderFunc<out T>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
