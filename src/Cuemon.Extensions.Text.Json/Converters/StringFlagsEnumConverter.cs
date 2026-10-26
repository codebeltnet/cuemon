using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json.Converters;
/// <summary>
/// Converter to convert enums with <see cref="FlagsAttribute"/> to and from strings.
/// </summary>
public class StringFlagsEnumConverter : JsonConverterFactory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringFlagsEnumConverter"/> class.
    /// </summary>
    public StringFlagsEnumConverter()
    {
    }

    /// <summary>
    /// Determines whether the type can be converted.
    /// </summary>
    /// <param name="typeToConvert">The type is checked as to whether it can be converted.</param>
    /// <returns><c>true</c> if the type can be converted, <c>false</c> otherwise.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum && typeToConvert.IsDefined(typeof(FlagsAttribute), false);
    }

    /// <summary>
    /// Creates a converter for a specified <paramref name="typeToConvert"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    /// <returns>
    /// An instance of a <see cref="JsonConverter{T}"/> where T is compatible with <paramref name="typeToConvert"/>.
    /// If <see langword="null"/> is returned, a <see cref="NotSupportedException"/> will be thrown.
    /// </returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new FlagsEnumConverter(typeToConvert);
    }
}

internal sealed class FlagsEnumConverter : JsonConverter<Enum>
{
    public FlagsEnumConverter(Type typeToConvert)
    {
        TypeToConvert = typeToConvert; // workaround after changes introduced with .NET 7 # MSBUG?
    }

    private Type TypeToConvert { get; }

    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = 0;
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
#if NET10_0_OR_GREATER
                    result |= (int)Enum.Parse(TypeToConvert, reader.GetString(), true);
#else
                    result |= (int)Enum.Parse(typeToConvert, reader.GetString(), true);
#endif
                    break;
            }
        }
#if NET10_0_OR_GREATER
        return Enum.ToObject(TypeToConvert, result) as Enum;
#else
        return Enum.ToObject(typeToConvert, result) as Enum;
#endif
    }

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteStartArray();
            writer.WriteNullValue();
            writer.WriteEndArray();
            return;
        }

        var enumName = value.ToString("G");
        var flags = enumName.Split(',');
        var enumType = value.GetType();
        if (enumType.IsDefined(typeof(FlagsAttribute), false))
        {
            writer.WriteStartArray();
            foreach (var flag in flags)
            {
                writer.WriteStringValue(options.SetPropertyName(flag.Trim()));
            }
            writer.WriteEndArray();
        }
    }
}
